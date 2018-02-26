using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class DatabaseWrapper
    {
        public List<Setting> m_settings { get; private set; }
        public List<Training> m_trainings { get; private set; }
        public List<Mitglied> m_mitglieder { get; private set; }
        public List<TrainerErnennung> m_trainerErnennungen { get; private set; }
        public List<Stundensatz> m_stundensaetze { get; private set; }
        public List<ZuschlagKindertraining> m_zuschlagKinderTraining { get; private set; }
        public List<TrainerStufe> m_trainerStufe { get; private set; }
        public List<Wohnung> m_wohnung { get; private set; }
        public List<Wohnungsbezug> m_wohnungsbezug { get; private set; }
        public List<Wochentag> m_wochentag { get; private set; }
        public List<Pruefung> m_pruefung { get; private set; }
        public List<Graduierung> m_graduierung { get; private set; }
        public List<Beitragsklasse> m_beitragsklasse { get; private set; }
        public List<Familienrabatt> m_familienrabatt { get; private set; }
        public List<TrainingsTeilnahme> m_trainingsTeilnahme { get; private set; }
        public List<Kurs> m_kurs { get; private set; }

        public async Task ReadTables( IUnityContainer Container, Action<string> tick )
        {
            tick("Trainings");
            m_trainings = await Container.Resolve<IRepository<Training>>().GetAsync();
            tick("Mitglieder");
            m_mitglieder = await Container.Resolve<IRepository<Mitglied>>().GetAsync();
            tick("TrainerErnennung");
            m_trainerErnennungen = await Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            tick("Stundensatz");
            m_stundensaetze = await Container.Resolve<IRepository<Stundensatz>>().GetAsync();
            tick("ZuschlagKindertraining");
            m_zuschlagKinderTraining = await Container.Resolve<IRepository<ZuschlagKindertraining>>().GetAsync();
            tick("TrainerStufe");
            m_trainerStufe = await Container.Resolve<IRepository<TrainerStufe>>().GetAsync();
            tick("Wohnung");
            m_wohnung = await Container.Resolve<IRepository<Wohnung>>().GetAsync();
            tick("Wohnungsbezug");
            m_wohnungsbezug = await Container.Resolve<IRepository<Wohnungsbezug>>().GetAsync();
            tick("Wochentag");
            m_wochentag = await Container.Resolve<IRepository<Wochentag>>().GetAsync();
            tick("Pruefung");
            m_pruefung = await Container.Resolve<IRepository<Pruefung>>().GetAsync();
            tick("Graduierung");
            m_graduierung = await Container.Resolve<IRepository<Graduierung>>().GetAsync();
            tick("Beitragsklasse");
            m_beitragsklasse = await Container.Resolve<IRepository<Beitragsklasse>>().GetAsync();
            tick("Familienrabatt");
            m_familienrabatt = await Container.Resolve<IRepository<Familienrabatt>>().GetAsync();
            tick("TrainingsTeilnahme");
            m_trainingsTeilnahme = await Container.Resolve<IRepository<TrainingsTeilnahme>>().GetAsync();
            tick("Kurs");
            m_kurs = await Container.Resolve<IRepository<Kurs>>().GetAsync();
            tick("Setting");
            m_settings = await Container.Resolve<IRepositoryBase<Setting, string>>().GetAsync();
            tick("");
            m_mitglieder.RemoveAt(0);   // Mitglied 0 is a dummy
            m_trainings = m_trainings.OrderBy(t => t.Termin).ToList();
        }

        public DateTime GetStartValidData()
        {
            string strStartValidData = m_settings.Single(s => s.Id == "DateValidData").Value;
            return DateTime.Parse(strStartValidData);
        }

        public string WeekDay(int id)
        {
            var wochentag = m_wochentag.Single(s => s.Id == id);
            return wochentag.Bezeichnung;
        }

        public Boolean IstNochMitglied(Mitglied member)
        {
             DateTime? austritt = member.Austritt;

            if (!austritt.HasValue)
                return true;

            return (austritt.Value.Year == 0);
        }

        public Boolean IstNochMitglied(int? id)
        {
            if (!id.HasValue)
                return false;

            if (id < 0)
                return false;

            return IstNochMitglied(MitgliedFromId(id.Value));
        }

        public List<Mitglied> CurrentMembers()
            => m_mitglieder.Where(m => IstNochMitglied(m)).ToList();

        public int MaxMitgliedsNr()
            => m_mitglieder.Max(t => t.Id);

        public int MinMitgliedsNr()
            => m_mitglieder.Min(t => t.Id);

        public int MaxKursNr()
            => m_kurs.Max(t => t.Id);

        public int MinKursNr()
            => m_kurs.Min(t => t.Id);

        public int MaxTrainingNr()
            => m_trainings.Max(t => t.Id);

        public int MinTrainingNr()
            => m_trainings.Min(t => t.Id);

        public Beitragsklasse BK(Mitglied mitglied) 
            => m_beitragsklasse.Single(s => s.Id == mitglied.BeitragsklasseID);

        public string BK_Text(Mitglied mitglied) 
            => BK(mitglied).BeitragsklasseRomanNumeral.ToString();

        public int Familienrabatt(Mitglied mitglied) 
            => m_familienrabatt.Single(s => s.Id == mitglied.Familienmitglied).Faktor;

        public string Trainerstufe(int i) 
            => m_trainerStufe.Single(s => s.Id == i).Bezeichnung;

        public int MaxTrainerstufe 
            => m_trainerStufe.Max(t => t.Id);

        public Mitglied MitgliedFromId(int id) 
            => m_mitglieder.Single(s => s.Id == id);

        public Training TrainingFromId(int id)
            => m_trainings.Single(s => s.Id == id);

        public int KursIdFromTrainingId(int id)
        {
            int? idKurs = TrainingFromId(id).KursID;    // Kurs NULL => 0
            return idKurs.HasValue ? idKurs.Value : 0;
        }

        public Boolean HatTeilgenommen(int member, Training training) 
            => m_trainingsTeilnahme.Exists(x => (x.MitgliedID == member) && (x.TrainingID == training.Id));

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

        public Kurs KursFromId(int id)
            => m_kurs.Single(s => s.Id == id);

        public Graduierung GraduierungFromId(int id)
            => m_graduierung.Single(s => s.Id == id);

        public DateTime DateFromTrainingParticipation(TrainingsTeilnahme tn)
            => TrainingFromId(tn.TrainingID).Termin;

        public bool DateIsInRange(DateTime date, DateTime datRangeStart, DateTime datRangeEnd)
            => (datRangeStart <= date) && (date <= datRangeEnd);

        public int NrOfTrainingsSince(int idMember, DateTime datStart)
            => m_trainingsTeilnahme.Count( p => (p.MitgliedID == idMember) && (DateFromTrainingParticipation(p) >= datStart) );

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, DateTime datStart)
            => list.Where(p => DateFromTrainingParticipation(p) >= datStart).ToList();

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, DateTime datStart, DateTime datEnd)
            => list.Where(p => DateIsInRange(DateFromTrainingParticipation(p), datStart, datEnd)).ToList();

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, int idMember)
            => list.Where(p => p.MitgliedID == idMember).ToList();

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, Func<TrainingsTeilnahme, bool> filter)
            => list.Where(p => filter(p)).ToList();

        public List<TrainingsTeilnahme> TrainingsTeilnahme(DateTime datStart, DateTime datEnd)
            => Filter( m_trainingsTeilnahme, datStart, datEnd).ToList();

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

        public List<Training> TrainingsInPeriod(DateTime datStart, DateTime datEnd) 
            => TrainingsInPeriod(null, datStart, datEnd);
    }
}