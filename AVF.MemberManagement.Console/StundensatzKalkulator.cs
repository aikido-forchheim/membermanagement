using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.Console
{
    internal class StundensatzKalkulator
    {
        public StundensatzKalkulator()
        {
        }

        public async Task Main2()
        {
            var trainings = await Program.Container.Resolve<IRepository<Training>>().GetAsync();
            var mitglieder = await Program.Container.Resolve<IRepository<Mitglied>>().GetAsync();
            var trainerErnennungen = await Program.Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            var stundensaetze = await Program.Container.Resolve<IRepository<Stundensatz>>().GetAsync();

            var aktuelleTrainerStufen = from trainerErnennung in trainerErnennungen
                group trainerErnennung by trainerErnennung.MitgliedID
                into trainerErnennungenGroup
                select new { MitgliedID = trainerErnennungenGroup.Key, AktuelleTrainerStufe = trainerErnennungenGroup.Min(t => t.StufeID)};

            var x = from training in trainings
                join trainerStufe in aktuelleTrainerStufen on training.Id equals trainerStufe.MitgliedID into
                    trainerStufenDerTrainings
                from trainerStufeDesTrainings in trainerStufenDerTrainings
                join mitglied in mitglieder on trainerStufeDesTrainings.MitgliedID equals mitglied.Id
                where training.Termin > new DateTime(2016, 01, 01) && training.Termin < new DateTime(2016, 02, 01)
                select new { Dauer = training.DauerMinuten, trainerStufeDesTrainings.AktuelleTrainerStufe, Name = $"{mitglied.Vorname}{mitglied.Nachname}" };

            //from trainingMitTrainerStufe in trainingsMitTrainerStufe
            // join Mitglied in mitglieder on trainingMitTrainerStufe.
            // where training.Termin > new DateTime(2016, 01, 01) && training.Termin < new DateTime(2016, 02, 01)
            // select new { Training = training, TrainerErnennung = trainerErnennung };
        }

        public async Task Main()
        {
            var trainings = await Program.Container.Resolve<IRepository<Training>>().GetAsync();
            var mitglieder = await Program.Container.Resolve<IRepository<Mitglied>>().GetAsync();
            var trainerErnennungen = await Program.Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            var stundensaetze = await Program.Container.Resolve<IRepository<Stundensatz>>().GetAsync();

            var trainingsJanuar = trainings.Where(training =>
                training.Termin > new DateTime(2016, 01, 01) && training.Termin < new DateTime(2016, 02, 01)).ToList();

            var hauptTrainierJanuar = trainingsJanuar.Select(training => training.Trainer).Distinct().ToList();

            foreach (var trainerId in hauptTrainierJanuar)
            {
                if (trainerId == -1) continue;

                var trainingsTrainerX = trainingsJanuar.Where(t => t.Trainer == trainerId).ToList();

                var name = mitglieder.Single(m => m.Id == trainerId).Nachname;
                var vorname = mitglieder.Single(m => m.Id == trainerId).Vorname;


                decimal entgelt = 0;
                var minutenGesamt = 0;

                foreach (var training in trainingsTrainerX)
                {
                    minutenGesamt += training.DauerMinuten;

                    var trainererstufe = trainerErnennungen.Where(t => t.MitgliedID == trainerId).Min(t => t.StufeID);

                    var stundensatz = stundensaetze.Single(s => s.TrainerStufenID == trainererstufe && s.TrainerNummer == 1 && s.Dauer == training.DauerMinuten); //s.TrainerNummer == 1 => Haupttrainer

                    entgelt += stundensatz.Betrag;
                }

                System.Console.WriteLine($"{vorname} {name}: {minutenGesamt}, {entgelt}");
            }
        }
    }
}