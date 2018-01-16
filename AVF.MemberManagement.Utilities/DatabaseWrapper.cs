using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.Utilities
{
    public class NrOfTrainings : IComparable<NrOfTrainings>
    {
        public int memberId;
        public int iCount;

        public NrOfTrainings(int m, int i)
        {
            memberId = m;
            iCount = i;
        }

        public void Increase() => ++iCount;

        public int CompareTo( NrOfTrainings other ) => other.iCount - iCount;
    };

    public class TrainingParticipation
    {
        private DatabaseWrapper m_db;

        private DateTime m_datStart;
        private DateTime m_datEnd;

        private struct Row
        {
            public int   idMember;
            public int[] aiValues;
        };

        private int m_iNrOfCols;
        private int m_iNrOfRows;

        private string[][] m_ColumnLabels;
        private Row[]      m_Matrix;

        private int[] m_RowSum;
        private int[] m_ColSum;

        private string FormatNumber(int i) 
            => (i > 0) ? $"{ i,7 }" : "       ";

        public string FormatMitglied(Mitglied mitglied)
            => $"{ mitglied.Nachname,-12 } { mitglied.Vorname,-12 } ({ mitglied.Id,3 }) ";

        public string FormatMitglied(int idMember)
            => FormatMitglied(m_db.MitgliedFromId(idMember));

        public TrainingParticipation( DatabaseWrapper db, DateTime datStart, DateTime datEnd )
        {
            m_db = db;
            m_datStart = datStart;
            m_datEnd = datEnd;

            int colKursNull = m_db.MaxKursNr();

            m_iNrOfRows = m_db.MaxMitgliedsNr() + 1; // One additional row for pseudo member with Id = -1
            m_iNrOfCols = m_db.MaxKursNr() + 1;      // One additional col for "Lehrgänge"

            m_ColumnLabels    = new string[2][];
            m_ColumnLabels[0] = new string[m_iNrOfCols];
            m_ColumnLabels[1] = new string[m_iNrOfCols];
            for (int iCol = 0; iCol < m_iNrOfCols - 1; ++iCol)
            {
                Kurs kurs = m_db.KursFromId( iCol + 1 );
                m_ColumnLabels[0][iCol] = $"    { db.WeekDay(kurs.WochentagID).Substring(0, 2) } ";
                m_ColumnLabels[1][iCol] = $" {kurs.Zeit:hh}:{kurs.Zeit:mm} ";
            }
            m_ColumnLabels[0][m_iNrOfCols - 1] = " Lehrgänge";
            m_ColumnLabels[1][m_iNrOfCols - 1] = " Sondertr.  Summe";

            m_Matrix = new Row[m_iNrOfRows];

            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
            {
                m_Matrix[iRow].aiValues = new int[m_iNrOfCols];
                m_Matrix[iRow].idMember = iRow;
            }

            m_RowSum = new int[m_iNrOfRows];
            m_ColSum = new int[m_iNrOfCols];

            foreach (var trainingsTeilnahme in m_db.TrainingsTeilnahme(m_datStart, m_datEnd))
            {
                int? idKurs = m_db.TrainingFromId(trainingsTeilnahme.TrainingID).KursID;
                int iCol = idKurs.HasValue ? (idKurs.Value-1) : colKursNull;
                int iRow = trainingsTeilnahme.MitgliedID;
                ++m_Matrix[iRow].aiValues[iCol];
                ++m_ColSum[iCol];
                ++m_RowSum[iRow];
            }

            Array.Sort(m_RowSum, m_Matrix);
            Array.Reverse(m_Matrix);
            Array.Reverse(m_RowSum);
        }

        public string[,] GetMatrix()
        {
            string[,] matrix;

            int iNrOfMatrixRows = 0;
            int iNrOfMatrixCols = m_iNrOfCols;

            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
                if (m_RowSum[iRow] > 0)
                    ++iNrOfMatrixRows;

            ++iNrOfMatrixRows;  // + 1 for col sum
            ++iNrOfMatrixCols;  // + 1 for row sum
            ++iNrOfMatrixCols;  // + 1 for member

            matrix = new string[iNrOfMatrixRows, iNrOfMatrixCols];

            int iStringRow = 0;
            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
            {
                if (m_RowSum[iRow] > 0)
                {
                    matrix[iStringRow, 0] = FormatMitglied(m_Matrix[iRow].idMember);
                    for (int iCol = 1; iCol < m_iNrOfCols; ++iCol)
                    {
                        matrix[iStringRow, iCol] = FormatNumber(m_Matrix[iRow].aiValues[iCol-1]);
                    }
                    matrix[iStringRow, iNrOfMatrixCols - 1] = FormatNumber(m_RowSum[iRow]);
                    ++iStringRow;
                }
            }
            matrix[iStringRow, 0] = "                     Insgesamt  ";
            for (int iCol = 1; iCol < iNrOfMatrixCols - 1; ++iCol)
            {
                matrix[iStringRow, iCol] = FormatNumber(m_ColSum[iCol-1]);
            }

            return matrix;
        }

        public string[][] GetColumnLabels() => m_ColumnLabels;
    }

    public class DatabaseWrapper
    {
        private List<Training> m_trainings;
        private List<Mitglied> m_mitglieder;
        private List<TrainerErnennung> m_trainerErnennungen;
        private List<Stundensatz> m_stundensaetze;
        private List<ZuschlagKindertraining> m_zuschlagKinderTraining;
        private List<TrainerStufe> m_trainerStufe;
        private List<Wohnung> m_wohnung;
        private List<Wohnungsbezug> m_wohnungsbezug;
        private List<Wochentag> m_wochentag;
        private List<Pruefung> m_pruefung;
        private List<Graduierung> m_graduierung;
        private List<Beitragsklasse> m_beitragsklasse;
        private List<Familienrabatt> m_familienrabatt;
        private List<TrainingsTeilnahme> m_trainingsTeilnahme;
        private List<Kurs> m_kurs;

        public async Task ReadTables( IUnityContainer Container )
        {
            m_trainings = await Container.Resolve<IRepository<Training>>().GetAsync();
            m_mitglieder = await Container.Resolve<IRepository<Mitglied>>().GetAsync();
            m_trainerErnennungen = await Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            m_stundensaetze = await Container.Resolve<IRepository<Stundensatz>>().GetAsync();
            m_zuschlagKinderTraining = await Container.Resolve<IRepository<ZuschlagKindertraining>>().GetAsync();
            m_trainerStufe = await Container.Resolve<IRepository<TrainerStufe>>().GetAsync();
            m_wohnung = await Container.Resolve<IRepository<Wohnung>>().GetAsync();
            m_wohnungsbezug = await Container.Resolve<IRepository<Wohnungsbezug>>().GetAsync();
            m_wochentag = await Container.Resolve<IRepository<Wochentag>>().GetAsync();
            m_pruefung = await Container.Resolve<IRepository<Pruefung>>().GetAsync();
            m_graduierung = await Container.Resolve<IRepository<Graduierung>>().GetAsync();
            m_beitragsklasse = await Container.Resolve<IRepository<Beitragsklasse>>().GetAsync();
            m_familienrabatt = await Container.Resolve<IRepository<Familienrabatt>>().GetAsync();
            m_trainingsTeilnahme = await Container.Resolve<IRepository<TrainingsTeilnahme>>().GetAsync();
            m_kurs = await Container.Resolve<IRepository<Kurs>>().GetAsync();
            m_mitglieder.RemoveAt(0);   // Mitglied 0 is a dummy
        }

        public int TrainerLevel(int? trainerId, DateTime termin)  // calculates level (Vereinstrainer, Lehrer ...) of a trainer at a given date
        {
            var ernennungen = m_trainerErnennungen.Where(t => (t.MitgliedID == trainerId) && ((t.Datum == null) || (t.Datum <= termin)));
            return ernennungen.Any() ? ernennungen.Max(t => t.StufeID) : 1;
        }

        public string WeekDay(int id)
        {
            var wochentag = m_wochentag.Single(s => s.Id == id);
            return wochentag.Bezeichnung;
        }

        public decimal TrainingAward(int level, Training training, int trainerNr)
        {
            decimal decResult = 0;
            if (!training.VHS)
            {
                var stundensatz = m_stundensaetze.Single(s => (s.TrainerStufenID == level) && (s.TrainerNummer == trainerNr) && (s.Dauer == training.DauerMinuten));
                decResult = stundensatz.Betrag;
            }
            return decResult;
        }

        public decimal ExtraAmount(Training training, int trainerNr)
        {
            if (training.Kindertraining)
            {
                var zuschlag = m_zuschlagKinderTraining.Single(s => (s.Trainernummer == trainerNr) && (s.Dauer == training.DauerMinuten));
                return zuschlag.Betrag;
            }
            return 0;
        }

        public int MaxMitgliedsNr()
        {
            return m_mitglieder.Max(t => t.Id);
        }

        public int MaxKursNr()
        {
            return m_kurs.Max(t => t.Id);
        }

        public Beitragsklasse BK(Mitglied mitglied)
        {
            return m_beitragsklasse.Single(s => s.Id == mitglied.BeitragsklasseID);
        }

        public string BK_Text(Mitglied mitglied)
        {
            return BK(mitglied).BeitragsklasseRomanNumeral.ToString();
        }

        public int Familienrabatt(Mitglied mitglied)
        {
            return m_familienrabatt.Single(s => s.Id == mitglied.Familienmitglied).Faktor;
        }

        public string Trainerstufe(int i)
        {
            return m_trainerStufe.Single(s => s.Id == i).Bezeichnung;
        }

        public int MaxTrainerstufe()
        {
            return m_trainerStufe.Max(t => t.Id);
        }

        public Mitglied MitgliedFromId(int id)
        {
            if (m_mitglieder.Exists(s => s.Id == id))
                return m_mitglieder.Single(s => s.Id == id);
            else
                return null;
        }

        public Training TrainingFromId(int id)
        {
            return m_trainings.Single(s => s.Id == id);
        }

        public Boolean HatTeilgenommen(int member, Training training)
        {
            return m_trainingsTeilnahme.Exists(x => (x.MitgliedID == member) && (x.TrainingID == training.Id));
        }

        public Boolean HatTeilgenommen(int member, List<Training> trainings)
        {
            bool found = false;
            foreach (var training in trainings)
            {
                if (HatTeilgenommen(member, training))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public int AnzahleBesuche(int member, List<Training> trainings)
        {
            int iCount = 0;
            foreach (var training in trainings)
            {
                if (HatTeilgenommen(member, training))
                    ++iCount;
            }

            return iCount;
        }

        public Boolean IstNochMitglied(int? id)
        {
            if (!id.HasValue)
                return false;

            if (id < 0)
                return false;

            DateTime? austritt = MitgliedFromId(id.Value).Austritt;

            if (!austritt.HasValue)
                return true;

            return (austritt.Value.Year == 0);
        }

        public List<Mitglied> Mitglieder()
            => m_mitglieder;

        public List<Kurs> Kurse()
            => m_kurs;

        public Kurs KursFromId(int id) 
            => m_kurs.Single(s => s.Id == id);
        
        public List<Pruefung> Pruefungen() 
            => m_pruefung;

        public Graduierung GraduierungFromId(int id) 
            => m_graduierung.Single(s => s.Id == id);
    
        public List<TrainingsTeilnahme> TrainingsTeilnahme( DateTime datStart, DateTime datEnd )
            => m_trainingsTeilnahme.Where(p => TrainingFromId(p.TrainingID).Termin > datStart && TrainingFromId(p.TrainingID).Termin < datEnd).ToList();

        public List<Training> Filter(List<Training> list, int? idKurs)
            => list.Where(training => training.KursID == idKurs).ToList();

        public List<Training> Filter(List<Training> list, DateTime datStart, DateTime datEnd)
            => list.Where(training => training.Termin > datStart && training.Termin < datEnd).ToList();

        public List<Training> TrainingsInPeriod( int ? idKurs, DateTime datStart, DateTime datEnd )
        {
            var result = Filter( m_trainings, datStart, datEnd );

            if ( idKurs != -1 )
                result = Filter( result, idKurs );

            return result.OrderBy(x => x.Termin).ToList();
        }

        public List<Training> TrainingsInPeriod( int? idKurs, int iJahr )
        {
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);

            return TrainingsInPeriod(idKurs, datStart, datEnd );
        }

        public List<Training> AllTrainings()
            => m_trainings;

        public decimal CalcTravelExpenses(int? idMitglied, DateTime termin)
        {
            var wohnungsbezug = m_wohnungsbezug.Where(t => (t.MitgliedId == idMitglied) && ((t.Datum == null) || (t.Datum <= termin)));
            if (wohnungsbezug.Any())
            {
                var letzterUmzug = wohnungsbezug.Max(t => t.Datum);
                var wohnungId = wohnungsbezug.Single(s => (s.Datum == letzterUmzug)).WohnungId;
                var wohnung = m_wohnung.Single(s => (s.Id == wohnungId));
                if (wohnung.Fahrtstrecke.HasValue)
                {
                    Decimal Cents = wohnung.Fahrtstrecke.Value * 17;
                    return Decimal.Floor(Cents + 0.5M) / 100;
                }
            }
            return 0;
        }
    }
}
