using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using Prism.Ioc;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Repositories
{
    public class RepositoriesTest : TestBase
    {
        [Fact]
        public async Task Beitragsklassen()
        {
            var beitragsklassen = await Bootstrapper.Container.Resolve<IRepository<Beitragsklasse>>().GetAsync();
            Assert.True(beitragsklassen.Count > 0);
        }

        [Fact]
        public async Task Familienrabatte()
        {
            var familienrabatte = await Bootstrapper.Container.Resolve<IRepository<Familienrabatt>>().GetAsync();
            Assert.True(familienrabatte.Count > 0);
        }

        [Fact]
        public async Task Graduierungen()
        {
            var graduierungen = await Bootstrapper.Container.Resolve<IRepository<Graduierung>>().GetAsync();
            Assert.True(graduierungen.Count > 0);
        }

        [Fact]
        public async Task Kurse()
        {
            var kurse = await Bootstrapper.Container.Resolve<IRepository<Kurs>>().GetAsync();
            Assert.True(kurse.Count > 0);
        }

        [Fact]
        public async Task Mitglieder()
        {
            var mitglieder = await Bootstrapper.Container.Resolve<IRepository<Mitglied>>().GetAsync();
            Assert.True(mitglieder.Count > 0);
        }

        [Fact]
        public async Task Pruefungen()
        {
            var pruefungen = await Bootstrapper.Container.Resolve<IRepository<Pruefung>>().GetAsync();
            Assert.True(pruefungen.Count > 0);
        }

        [Fact]
        public async Task Settings()
        {
            var settings = await Bootstrapper.Container.Resolve<IRepositoryBase<Setting, string>>().GetAsync();
            Assert.True(settings.Count > 0);
        }

        [Fact]
        public async Task Stundensaetze()
        {
            var stundensaetze = await Bootstrapper.Container.Resolve<IRepository<Stundensatz>>().GetAsync();
            Assert.True(stundensaetze.Count > 0);
        }

        [Fact]
        public async Task Tests()
        {
            var tests = await Bootstrapper.Container.Resolve<IRepository<Test>>().GetAsync();
            Assert.True(tests.Count > 0);
        }

        [Fact]
        public async Task TrainerErnennungen()
        {
            var trainerErnennungen = await Bootstrapper.Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            Assert.True(trainerErnennungen.Count > 0);
        }

        [Fact]
        public async Task TrainerStufen()
        {
            var trainerStufen = await Bootstrapper.Container.Resolve<IRepository<TrainerStufe>>().GetAsync();
            Assert.True(trainerStufen.Count > 0);
        }

        [Fact]
        public async Task Trainings()
        {
            var trainings = await Bootstrapper.Container.Resolve<IRepository<Training>>().GetAsync();
            Assert.True(trainings.Count > 0);
        }

        [Fact]
        public async Task TrainingsTeilnahmen()
        {
            //var start = DateTime.Now;
            var trainingsTeilnahmen = await Bootstrapper.Container.Resolve<IRepository<TrainingsTeilnahme>>().GetAsync();
            //var end = DateTime.Now - start;
            Assert.True(trainingsTeilnahmen.Count > 0);
        }

        [Fact]
        public async Task Users()
        {
            var users = await Bootstrapper.Container.Resolve<IRepository<User>>().GetAsync();
            Assert.True(users.Count > 0);
        }

        [Fact]
        public async Task Wochentage()
        {
            var wochentage = await Bootstrapper.Container.Resolve<IRepository<Wochentag>>().GetAsync();
            Assert.True(wochentage.Count > 0);
        }

        [Fact]
        public async Task Wohungen()
        {
            var wohnungen = await Bootstrapper.Container.Resolve<IRepository<Wohnung>>().GetAsync();
            Assert.True(wohnungen.Count > 0);
        }

        [Fact]
        public async Task Wohnungsbezug()
        {
            var wohnungsbezug = await Bootstrapper.Container.Resolve<IRepository<Wohnungsbezug>>().GetAsync();
            Assert.True(wohnungsbezug.Count > 0);
        }

        [Fact]
        public async Task ZuschlaegeKindertraining()
        {
            var zuschlaegeKindertraining = await Bootstrapper.Container.Resolve<IRepository<ZuschlagKindertraining>>().GetAsync();
            Assert.True(zuschlaegeKindertraining.Count > 0);
        }
    }
}
