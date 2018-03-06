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
        public List<Setting> P_settings { get; private set; }
        public List<Training> P_trainings { get; private set; }
        public List<Mitglied> P_mitglieder { get; private set; }
        public List<TrainerErnennung> P_trainerErnennungen { get; private set; }
        public List<Stundensatz> P_stundensaetze { get; private set; }
        public List<ZuschlagKindertraining> P_zuschlagKinderTraining { get; private set; }
        public List<TrainerStufe> P_trainerStufe { get; private set; }
        public List<Wohnung> P_wohnung { get; private set; }
        public List<Wohnungsbezug> P_wohnungsbezug { get; private set; }
        public List<Wochentag> P_wochentag { get; private set; }
        public List<Pruefung> P_pruefung { get; private set; }
        public List<Graduierung> P_graduierung { get; private set; }
        public List<Beitragsklasse> P_beitragsklasse { get; private set; }
        public List<Familienrabatt> P_familienrabatt { get; private set; }
        public List<TrainingsTeilnahme> P_trainingsTeilnahme { get; private set; }
        public List<Kurs> P_kurs { get; private set; }

        public async Task ReadTables( IUnityContainer Container, Action<string> tick )
        {
            tick("Trainings");
            P_trainings = await Container.Resolve<IRepository<Training>>().GetAsync();
            tick("Mitglieder");
            P_mitglieder = await Container.Resolve<IRepository<Mitglied>>().GetAsync();
            tick("TrainerErnennung");
            P_trainerErnennungen = await Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            tick("Stundensatz");
            P_stundensaetze = await Container.Resolve<IRepository<Stundensatz>>().GetAsync();
            tick("ZuschlagKindertraining");
            P_zuschlagKinderTraining = await Container.Resolve<IRepository<ZuschlagKindertraining>>().GetAsync();
            tick("TrainerStufe");
            P_trainerStufe = await Container.Resolve<IRepository<TrainerStufe>>().GetAsync();
            tick("Wohnung");
            P_wohnung = await Container.Resolve<IRepository<Wohnung>>().GetAsync();
            tick("Wohnungsbezug");
            P_wohnungsbezug = await Container.Resolve<IRepository<Wohnungsbezug>>().GetAsync();
            tick("Wochentag");
            P_wochentag = await Container.Resolve<IRepository<Wochentag>>().GetAsync();
            tick("Pruefung");
            P_pruefung = await Container.Resolve<IRepository<Pruefung>>().GetAsync();
            tick("Graduierung");
            P_graduierung = await Container.Resolve<IRepository<Graduierung>>().GetAsync();
            tick("Beitragsklasse");
            P_beitragsklasse = await Container.Resolve<IRepository<Beitragsklasse>>().GetAsync();
            tick("Familienrabatt");
            P_familienrabatt = await Container.Resolve<IRepository<Familienrabatt>>().GetAsync();
            tick("TrainingsTeilnahme");
            P_trainingsTeilnahme = await Container.Resolve<IRepository<TrainingsTeilnahme>>().GetAsync();
            tick("Kurs");
            P_kurs = await Container.Resolve<IRepository<Kurs>>().GetAsync();
            tick("Settings");
            P_settings = await Container.Resolve<IRepositoryBase<Setting, string>>().GetAsync();
            tick("");
            P_mitglieder.RemoveAt(0);   // Mitglied 0 is a dummy
            P_trainings = P_trainings.OrderBy(t => t.Termin).ToList();

            // Add pseude course 0 for trainings without course

            Kurs kurs0 = new Kurs();
            kurs0.Id = 0;
            kurs0.Zeit = TimeSpan.Zero;

            P_kurs.Add(kurs0);

            foreach (Training t in P_trainings)
            {
                if (!t.KursID.HasValue)
                    t.KursID = 0;
            }

            tick("Finished ....");
        }

        public DateTime GetStartValidData()
        {
            string strStartValidData = P_settings.Single(s => s.Id == "DateValidData").Value;
            return DateTime.Parse(strStartValidData);
        }

        public string WeekDay(int id)
        {
            var wochentag = P_wochentag.Single(s => s.Id == id);
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
            => P_mitglieder.Where(m => IstNochMitglied(m)).ToList();

        public int MaxMitgliedsNr()
            => P_mitglieder.Max(t => t.Id);

        public int MinMitgliedsNr()
            => P_mitglieder.Min(t => t.Id);

        public int MaxKursNr()
            => P_kurs.Max(t => t.Id);

        public int MinKursNr()
            => P_kurs.Min(t => t.Id);

        public int MaxTrainingNr()
            => P_trainings.Max(t => t.Id);

        public int MinTrainingNr()
            => P_trainings.Min(t => t.Id);

        public Beitragsklasse BK(Mitglied mitglied) 
            => P_beitragsklasse.Single(s => s.Id == mitglied.BeitragsklasseID);

        public string BK_Text(Mitglied mitglied) 
            => BK(mitglied).BeitragsklasseRomanNumeral.ToString();

        public int Familienrabatt(Mitglied mitglied) 
            => P_familienrabatt.Single(s => s.Id == mitglied.Familienmitglied).Faktor;

        public string Trainerstufe(int i) 
            => P_trainerStufe.Single(s => s.Id == i).Bezeichnung;

        public int MaxTrainerstufe 
            => P_trainerStufe.Max(t => t.Id);

        public Mitglied MitgliedFromId(int id)
            => P_mitglieder.Single(s => s.Id == id);

        public Training TrainingFromId(int id)
            => P_trainings.Single(s => s.Id == id);

        public DateTime TerminFromTrainingId(int id)
            => TrainingFromId(id).Termin;

        public int KursIdFromTrainingId(int id)
            => TrainingFromId(id).KursID.Value;

        public Boolean HatTeilgenommen(int member, Training training) 
            => P_trainingsTeilnahme.Exists(x => (x.MitgliedID == member) && (x.TrainingID == training.Id));

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
            => P_kurs.Single(s => s.Id == id);

        public Graduierung GraduierungFromId(int id)
            => P_graduierung.Single(s => s.Id == id);

        public DateTime DateFromTrainingParticipation(TrainingsTeilnahme tn)
            => TrainingFromId(tn.TrainingID).Termin;

        public bool DateIsInRange(TrainingsTeilnahme tn, DateTime datRangeStart, DateTime datRangeEnd)
            => DateIsInRange(DateFromTrainingParticipation(tn), datRangeStart, datRangeEnd);

        public bool DateIsInRange(DateTime date, DateTime datRangeStart, DateTime datRangeEnd)
            => (datRangeStart <= date) && (date <= datRangeEnd);

        public int NrOfTrainingsSince(int idMember, DateTime datStart)
            => P_trainingsTeilnahme.Count( p => (p.MitgliedID == idMember) && (DateFromTrainingParticipation(p) >= datStart) );

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, DateTime datStart)
            => list.Where(p => DateFromTrainingParticipation(p) >= datStart).ToList();

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, DateTime datStart, DateTime datEnd)
            => list.Where(p => DateIsInRange(DateFromTrainingParticipation(p), datStart, datEnd)).ToList();

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, int idMember)
            => list.Where(p => p.MitgliedID == idMember).ToList();

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, Func<TrainingsTeilnahme, bool> filter)
            => list.Where(p => filter(p)).ToList();

        public List<TrainingsTeilnahme> TrainingsTeilnahme(DateTime datStart, DateTime datEnd)
            => Filter( P_trainingsTeilnahme, datStart, datEnd).ToList();

        public List<Training> Filter(List<Training> list, int? idKurs)
            => list.Where(training => training.KursID == idKurs).ToList();

        public List<Training> Filter(List<Training> list, DateTime datStart, DateTime datEnd)
            => list.Where(training => training.Termin > datStart && training.Termin < datEnd).ToList();

        public List<Training> TrainingsInPeriod( int ? idKurs, DateTime datStart, DateTime datEnd )
        {
            var result = Filter( P_trainings, datStart, datEnd );

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