using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Repositories
{
    public class RepositoriesTest : TestBase
    {
        [Fact]
        public async void Beitragsklassen()
        {
            var beitragsklassen = await Bootstrapper.Container.Resolve<IRepository<Beitragsklasse>>().GetAsync();
            Assert.True(beitragsklassen.Count > 0);
        }

        [Fact]
        public async void Familienrabatte()
        {
            var familienrabatte = await Bootstrapper.Container.Resolve<IRepository<Familienrabatt>>().GetAsync();
            Assert.True(familienrabatte.Count > 0);
        }

        [Fact]
        public async void Graduierungen()
        {
            var graduierungen = await Bootstrapper.Container.Resolve<IRepository<Graduierung>>().GetAsync();
            Assert.True(graduierungen.Count > 0);
        }

        [Fact]
        public async void Kurse()
        {
            var kurse = await Bootstrapper.Container.Resolve<IRepository<Kurs>>().GetAsync();
            Assert.True(kurse.Count > 0);
        }

        [Fact]
        public async void Mitglieder()
        {
            var mitglieder = await Bootstrapper.Container.Resolve<IRepository<Mitglied>>().GetAsync();
            Assert.True(mitglieder.Count > 0);
        }

        [Fact]
        public async void Pruefungen()
        {
            var pruefungen = await Bootstrapper.Container.Resolve<IRepository<Pruefung>>().GetAsync();
            Assert.True(pruefungen.Count > 0);
        }

        [Fact]
        public async void Settings()
        {
            var settings = await Bootstrapper.Container.Resolve<IRepositoryBase<Setting, string>>().GetAsync();
            Assert.True(settings.Count > 0);
        }

        [Fact]
        public async void Stundensaetze()
        {
            var stundensaetze = await Bootstrapper.Container.Resolve<IRepository<Stundensatz>>().GetAsync();
            Assert.True(stundensaetze.Count > 0);
        }

        [Fact]
        public async void Tests()
        {
            var tests = await Bootstrapper.Container.Resolve<IRepository<Test>>().GetAsync();
            Assert.True(tests.Count > 0);
        }

        [Fact]
        public async void TrainerErnennungen()
        {
            var trainerErnennungen = await Bootstrapper.Container.Resolve<IRepository<TrainerErnennung>>().GetAsync();
            Assert.True(trainerErnennungen.Count > 0);
        }

        [Fact]
        public async void TrainerStufen()
        {
            var trainerStufen = await Bootstrapper.Container.Resolve<IRepository<TrainerStufe>>().GetAsync();
            Assert.True(trainerStufen.Count > 0);
        }

        [Fact]
        public async void Trainings()
        {
            var trainings = await Bootstrapper.Container.Resolve<IRepository<Training>>().GetAsync();
            Assert.True(trainings.Count > 0);
        }

        [Fact]
        public async void TrainingsTeilnahmen()
        {
            //var start = DateTime.Now;
            var trainingsTeilnahmen = await Bootstrapper.Container.Resolve<IRepository<TrainingsTeilnahme>>().GetAsync();
            //var end = DateTime.Now - start;
            Assert.True(trainingsTeilnahmen.Count > 0);
        }

        [Fact]
        public async void Users()
        {
            var users = await Bootstrapper.Container.Resolve<IRepository<User>>().GetAsync();
            Assert.True(users.Count > 0);
        }

        [Fact]
        public async void Wochentage()
        {
            var wochentage = await Bootstrapper.Container.Resolve<IRepository<Wochentag>>().GetAsync();
            Assert.True(wochentage.Count > 0);
        }

        [Fact]
        public async void Wohungen()
        {
            var wohungen = await Bootstrapper.Container.Resolve<IRepository<Wohnung>>().GetAsync();
            Assert.True(wohungen.Count > 0);
        }

        [Fact]
        public async void ZuschlaegeKindertraining()
        {
            var zuschlaegeKindertraining = await Bootstrapper.Container.Resolve<IRepository<ZuschlagKindertraining>>().GetAsync();
            Assert.True(zuschlaegeKindertraining.Count > 0);
        }
    }
}
