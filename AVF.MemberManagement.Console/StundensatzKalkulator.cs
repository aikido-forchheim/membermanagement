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

            var trainingsJanuar = trainings.Where(training =>
                training.Termin > new DateTime(2016, 01, 01) && training.Termin < new DateTime(2016, 02, 01)).ToList();

            var trainierJanuar = trainingsJanuar.Select(training => training.Trainer).Distinct().ToList();

            foreach (var trainerId in trainierJanuar)
            {
                var trainingsTrainerX = trainingsJanuar.Where(t => t.Trainer == trainerId).ToList();

                var minutenGesamt = trainingsTrainerX.Sum(t => t.DauerMinuten);

                var name = mitglieder.Single(m => m.Id == trainerId).Nachname;
                var vorname = mitglieder.Single(m => m.Id == trainerId).Vorname;

                System.Console.WriteLine($"{vorname} {name}: {minutenGesamt}");
            }
        }
    }
}