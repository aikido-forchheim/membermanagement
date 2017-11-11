using System;
using System.Linq;
using System.Threading.Tasks;
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