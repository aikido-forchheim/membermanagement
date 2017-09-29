using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Xunit;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class SettingsProxyTest : TestBase
    {
        [Fact]
        public async Task CanGetOneSetting()
        {
            var settingsProxy = Bootstrapper.Container.Resolve<ISettingsProxy>();

            var settings = await settingsProxy.LoadSettingsCacheAsync();

            var canGetOneSetting = settings != null && settings.Any();

            Assert.True(canGetOneSetting);
        }

        [Fact]
        public async Task TestForceCacheReload()
        {
            var settingsProxy = Bootstrapper.Container.Resolve<ISettingsProxy>();

            var settings = await settingsProxy.LoadSettingsCacheAsync();

            var settings2 = await settingsProxy.LoadSettingsCacheAsync(true);

            Assert.True(settings != settings2);
        }

        [Fact]
        public async Task TestCacheUse()
        {
            var settingsProxy = Bootstrapper.Container.Resolve<ISettingsProxy>();

            var settings = await settingsProxy.LoadSettingsCacheAsync();

            var settings2 = await settingsProxy.LoadSettingsCacheAsync();

            Assert.True(settings == settings2);
        }
    }
}
