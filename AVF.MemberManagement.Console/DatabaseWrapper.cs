using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.Console
{
    internal class DatabaseWrapper
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

        public async Task ReadTables()
        {
            m_trainings = await Program.Container.Resolve<IRepository<Training>>().GetAsync();
            m_mitglieder = await Program.Container.Resolve<IRepository<Mitglied>>().GetAsync();
            m_trainerErnennungen = await Program.Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            m_stundensaetze = await Program.Container.Resolve<IRepository<Stundensatz>>().GetAsync();
            m_zuschlagKinderTraining = await Program.Container.Resolve<IRepository<ZuschlagKindertraining>>().GetAsync();
            m_trainerStufe = await Program.Container.Resolve<IRepository<TrainerStufe>>().GetAsync();
            m_wohnung = await Program.Container.Resolve<IRepository<Wohnung>>().GetAsync();
            m_wohnungsbezug = await Program.Container.Resolve<IRepository<Wohnungsbezug>>().GetAsync();
            m_wochentag = await Program.Container.Resolve<IRepository<Wochentag>>().GetAsync();
            m_pruefung = await Program.Container.Resolve<IRepository<Pruefung>>().GetAsync();
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
            return m_mitglieder.Single(s => s.Id == id);
        }

        public List<Mitglied> Mitglieder()
        {
            return m_mitglieder;
        }

        public List<Pruefung> Pruefungen()
        {
            return m_pruefung;
        }

        public List<Training> Prepare(int iJahr)
        {
            int iArraySize = MaxMitgliedsNr() + 1;

            DateTime datStart = new DateTime(iJahr,  1,  1);
            DateTime datEnd   = new DateTime(iJahr, 12, 31);

            return m_trainings.Where(training => training.Termin > datStart && training.Termin < datEnd).ToList();
        }

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
                    return Decimal.Round(wohnung.Fahrtstrecke.Value * 0.17M, 2);
                }
            }
            return 0;
        }
    }
}
