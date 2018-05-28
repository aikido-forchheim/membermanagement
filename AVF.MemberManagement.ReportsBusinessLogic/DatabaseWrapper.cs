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
            tick("** Vorbereitungen **");
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

            tick("");
        }

        public DateTime GetStartValidData()
        {
            string strStartValidData = P_settings.Single(s => s.Id == "DateValidData").Value;
            return new DateTime(2013,1,1); // DateTime.Parse(strStartValidData);
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

        public Boolean IstAktivesMitglied(Mitglied member)
            => IstNochMitglied(member) && (member.BeitragsklasseID != 3); // BK III: passiv

        public List<Mitglied> CurrentMembers()
            => P_mitglieder.Where(m => IstNochMitglied(m)).ToList();

        public List<Mitglied> ActiveMembers()
            => P_mitglieder.Where(m => IstAktivesMitglied(m)).ToList();

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

        public DateTime EntryDateFromMemberId(int id)
            => MitgliedFromId(id).Eintritt.Value;

        public int YearsOfMembership(int id)
        {
            Mitglied member = MitgliedFromId(id);
            if (member.Eintritt.HasValue)
                return DateTime.Now.Year - member.Eintritt.Value.Year;
            else
                return 9999;
        }

        public Wohnung Wohnung(Mitglied member)
        {
            List<Wohnungsbezug> list = P_wohnungsbezug.Where(b => b.MitgliedId == member.Id).ToList();
            Wohnungsbezug bezug = list.OrderByDescending(b => b.Datum).FirstOrDefault();
            return (bezug == null) ? null : P_wohnung.Single(w => w.Id == bezug.WohnungId);
        }

        public Training TrainingFromId(int id)
            => P_trainings.Single(s => s.Id == id);

        public DateTime TerminFromTrainingId(int id)
            => TrainingFromId(id).Termin;

        public int KursIdFromTrainingId(int id)
            => TrainingFromId(id).KursID.Value;

        public Boolean HatTeilgenommen(int member, Training training) 
            => P_trainingsTeilnahme.Exists(x => (x.MitgliedID == member) && (x.TrainingID == training.Id));

        public Boolean HatTeilgenommen(int member, List<Training> trainings)
            => trainings.Exists(t => HatTeilgenommen(member, t));

        public int AnzahleBesuche(int member, List<Training> trainings)
            => trainings.Count(t => HatTeilgenommen(member, t));

        public Kurs KursFromId(int id)
            => P_kurs.Single(s => s.Id == id);

        public Graduierung GraduierungFromId(int id)
            => P_graduierung.Single(s => s.Id == id);

        public DateTime DateFromTrainingParticipation(TrainingsTeilnahme tn)
            => TrainingFromId(tn.TrainingID).Termin;

        public int NrOfTrainingsInRange(int idMember, TimeRange range)
            => P_trainingsTeilnahme.Count(p => (p.MitgliedID == idMember) && range.Includes(DateFromTrainingParticipation(p)));

        public int NrOfTrainingsSince(int idMember, DateTime datStart)
            => P_trainingsTeilnahme.Count(p => (p.MitgliedID == idMember) && (DateFromTrainingParticipation(p) >= datStart));

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, int idMember)
            => list.Where(p => p.MitgliedID == idMember).ToList();

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, TimeRange range)
            => list.Where(p => range.Includes(DateFromTrainingParticipation(p))).ToList();

        public List<TrainingsTeilnahme> Filter(List<TrainingsTeilnahme> list, Func<TrainingsTeilnahme, bool> filter)
            => list.Where(p => filter(p)).ToList();

        public List<Training> Filter(List<Training> list, int? idKurs)
            => list.Where(training => training.KursID == idKurs).ToList();

        public List<Training> Filter(List<Training> list, TimeRange range)
            => list.Where(training => range.Includes(training.Termin)).ToList();

        public List<Training> TrainingsInPeriod(int? idKurs, TimeRange range)
        {
            var result = Filter(P_trainings, range);

            if (idKurs != Globals.ALL_COURSES)
                result = Filter(result, idKurs);

            return result.OrderBy(x => x.Termin).ToList();
        }

        public List<Training> TrainingsInPeriod( int? idKurs, int iJahr )
            => TrainingsInPeriod(idKurs, new TimeRange(iJahr));
    }
}