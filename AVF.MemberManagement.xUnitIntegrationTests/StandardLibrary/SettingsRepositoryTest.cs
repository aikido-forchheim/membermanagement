using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class SettingsRepositoryTest : TestBase
    {
        [Fact]
        public async void GetTest()
        {
            var settingsRepository = Bootstrapper.Container.Resolve<IRepositoryBase<Setting, string>>();

            var settings = await settingsRepository.GetAsync();

            Assert.True(settings.Count > 1);
        }

        [Fact]
        public async void GetSingleValueTest()
        {
            var settingsRepository = Bootstrapper.Container.Resolve<IRepositoryBase<Setting, string>>();

            var setting = await settingsRepository.GetAsync("Logo");

            Assert.True(setting != null);
        }

        [Fact]
        public async void GetSingleValueNotExistingValueTest()
        {
            var settingsRepository = Bootstrapper.Container.Resolve<IRepositoryBase<Setting, string>>();

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => { await settingsRepository.GetAsync("qwertz"); });
        }

        [Fact]
        public async Task TestCacheUse()
        {
            var settingsRepository = Bootstrapper.Container.Resolve<IRepositoryBase<Setting,string>>();

            var settings = await settingsRepository.GetAsync();

            var settings2 = await settingsRepository.GetAsync();

            Assert.True(settings.First() == settings2.First());
        }

        //RestApiSettingsPageViewModel directly uses the SettingsProxy without cache now, so we do not need a ResetCache method currently
        //public async Task TestForceCacheReload()
    }
}
