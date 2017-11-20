using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.Console
{
    internal class StundensatzKalkulator
    {
        private List<Training>               m_trainings;
        private List<Mitglied>               m_mitglieder;
        private List<TrainerErnennung>       m_trainerErnennungen;
        private List<Stundensatz>            m_stundensaetze;
        private List<ZuschlagKindertraining> m_zuschlagKinderTraining;
        private List<TrainerStufe>           m_trainerStufe;
        private List<Wohnung>                m_wohnung;
        private List<Wohnungsbezug>          m_wohnungsbezug;
        private List<Wochentag>              m_wochentag;

        struct TrainerVerguetung
        {
            public decimal decVerguetung;
            public decimal decFahrtkosten;
        };

        private TrainerVerguetung[] m_aTrData;

        private int TrainerLevel( int? trainerId, DateTime termin )  // calculates level (Vereinstrainer, Lehrer ...) of a trainer at a given date
        {
            var ernennungen = m_trainerErnennungen.Where(t => (t.MitgliedID == trainerId) && ((t.Datum == null) || (t.Datum <= termin)));
            return ernennungen.Any() ? ernennungen.Max(t => t.StufeID) : 1;
        }

        private string WeekDay(int id)
        {
            var wochentag = m_wochentag.Single(s => s.Id == id);
            return wochentag.Bezeichnung;
        }

        private decimal TrainingAward( int level, int duration, int trainerNr )
        {
            var stundensatz = m_stundensaetze.Single(s => (s.TrainerStufenID == level) && (s.TrainerNummer == trainerNr) && (s.Dauer == duration));
            return stundensatz.Betrag;
        }

        private decimal ExtraAmount( Training training, int trainerNr )
        {
            if (training.Kindertraining)
            {
                var zuschlag = m_zuschlagKinderTraining.Single(s => (s.Trainernummer == trainerNr) && (s.Dauer == training.DauerMinuten));
                return zuschlag.Betrag;
            }
            return 0;
        }

        private decimal? Distance( int? idMitglied, DateTime termin )
        {
            var wohnungsbezug = m_wohnungsbezug.Where(t => (t.MitgliedId == idMitglied) && ((t.Datum == null) || (t.Datum <= termin)));
            if (wohnungsbezug.Any())
            {
                var letzterUmzug = wohnungsbezug.Max(t => t.Datum);
                var wohnungId = wohnungsbezug.Single(s => (s.Datum == letzterUmzug)).WohnungId;
                var wohnung = m_wohnung.Single(s => (s.Id == wohnungId));
                if (wohnung.Fahrtstrecke.HasValue)
                    return wohnung.Fahrtstrecke.Value;
            }
            return null;
        }

        private decimal TravelExpenses(int? idMitglied, DateTime termin)
        {
            decimal? fahrtstrecke = Distance( idMitglied, termin );
            if (fahrtstrecke.HasValue)
            {
                return Decimal.Round(fahrtstrecke.Value * 0.17M, 2);
            }
            return 0;
        }

        private void WriteTraining(System.IO.StreamWriter ofile, Training training)
        {
            ofile.Write($"{training.Termin:yyyy-MM-dd} {WeekDay(training.WochentagID),-10} {training.Zeit:hh}:{training.Zeit:mm}, {training.DauerMinuten,3} min, ");
        }

        private void WriteMitglied(System.IO.StreamWriter ofile, Mitglied mitglied)
        {
            ofile.Write($"{ mitglied.Nachname,-10 } { mitglied.Vorname,-10 } ({ mitglied.Id,3 }) ");
        }

        private void AddTrainerVerguetung( System.IO.StreamWriter ofile, Training training, int? trainerId, int trainerNr )
        {
            if ( ! trainerId.HasValue ) return;
            if (   trainerId < 0 )      return;

            WriteTraining( ofile, training );

            ofile.Write( $"{trainerNr}. Trainer, " );

            var trainerstufe = TrainerLevel(trainerId, training.Termin);

            WriteMitglied( ofile, m_mitglieder.Single(s => s.Id == trainerId));
            ofile.Write($" Stufe {trainerstufe}, ");

            decimal summe = 0;

            // Vergütung für Unterrichtstätigkeit

            if ( training.VHS )
            {
                ofile.Write($" -------- VHS --------  ");
            }
            else
            {
                decimal grundVerguetung        = TrainingAward( trainerstufe, training.DauerMinuten, trainerNr );
                decimal zuschlagKinderTraining = ExtraAmount  ( training, trainerNr );
                decimal gesamtVerguetung       = grundVerguetung + zuschlagKinderTraining;

                m_aTrData[trainerId.Value].decVerguetung += gesamtVerguetung;
                summe += gesamtVerguetung;

                ofile.Write($"{ grundVerguetung,5 }€");
                if (zuschlagKinderTraining > 0 )
                    ofile.Write($" + { zuschlagKinderTraining, 4 }€");
                else
                    ofile.Write($"        ");
                ofile.Write($" = { gesamtVerguetung,5 }€ ");
            }

            // Fahrtkosten

            decimal fahrtKosten = TravelExpenses(trainerId, training.Termin);

            m_aTrData[trainerId.Value].decFahrtkosten += fahrtKosten;
            summe += fahrtKosten;

            if ( fahrtKosten > 0 )
                ofile.Write($"FKZ = { fahrtKosten, 5 }€");
            else
                ofile.Write($" ?????????? ");

            ofile.Write($"  Summe = { summe, 5 }€");
            ofile.WriteLine();
        }

        private void GatherData( System.IO.StreamWriter ofile )
        {
            DateTime periodStart = new DateTime(2016, 1, 1);
            DateTime periodEnd = new DateTime(2016, 12, 31);

            var trainingsInPeriod = m_trainings.Where(training => training.Termin > periodStart && training.Termin < periodEnd).ToList();

            foreach (var training in trainingsInPeriod)
            {
                AddTrainerVerguetung( ofile, training, training.Trainer,    1 );
                AddTrainerVerguetung( ofile, training, training.Kotrainer1, 2 );
                AddTrainerVerguetung( ofile, training, training.Kotrainer2, 3 );
            }
        }

        private void WriteSummary( System.IO.StreamWriter ofile )
        {
            ofile.WriteLine();

            ofile.WriteLine($"Nachname   Vorname     Nr.  Unterricht   FKZ      Summe");
            ofile.WriteLine();

            decimal summeUnterricht = 0;
            decimal summeFKZ = 0;
            foreach (Mitglied mitglied in m_mitglieder)
            {
                if (mitglied.Id >= 0)
                {
                    decimal betrag = m_aTrData[mitglied.Id].decVerguetung;
                    decimal FKZ    = m_aTrData[mitglied.Id].decFahrtkosten;

                    if (betrag > 0)
                    {
                        WriteMitglied( ofile, mitglied );
                        ofile.WriteLine($" { betrag, 8 }€ { FKZ, 8 }€ { betrag + FKZ, 8 }€");
                        summeUnterricht += betrag;
                        summeFKZ += FKZ;
                    }
                }
            }

            ofile.WriteLine();
            ofile.WriteLine($"Gesamtbeträge               { summeUnterricht,8 }€ { summeFKZ,8 }€  { summeUnterricht + summeFKZ,8 }€");
        }

        public async Task Main()
        {
            m_trainings              = await Program.Container.Resolve < IRepository < Training >               > ().GetAsync();
            m_mitglieder             = await Program.Container.Resolve < IRepository < Mitglied >               > ().GetAsync();
            m_trainerErnennungen     = await Program.Container.Resolve < IRepository < TrainerErnennung >       > ().GetAsync();
            m_stundensaetze          = await Program.Container.Resolve < IRepository < Stundensatz >            > ().GetAsync();
            m_zuschlagKinderTraining = await Program.Container.Resolve < IRepository < ZuschlagKindertraining > > ().GetAsync();
            m_trainerStufe           = await Program.Container.Resolve < IRepository < TrainerStufe>            > ().GetAsync();
            m_wohnung                = await Program.Container.Resolve < IRepository < Wohnung >                > ().GetAsync();
            m_wohnungsbezug          = await Program.Container.Resolve < IRepository < Wohnungsbezug >          > ().GetAsync();
            m_wochentag              = await Program.Container.Resolve < IRepository < Wochentag>               > ().GetAsync();

            int iArraySize = m_mitglieder.Max(t => t.Id) + 1;

            m_aTrData = new TrainerVerguetung[iArraySize];

            System.IO.StreamWriter ofile = new System.IO.StreamWriter( @"Trainerverguetung.txt" );

            GatherData( ofile );

            WriteSummary( ofile );

            ofile.Close();
        }
    }
}