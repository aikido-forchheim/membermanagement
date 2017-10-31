using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class SettingsProxyTest : TestBase
    {
        [Fact]
        public async Task CanGetOneSetting()
        {
            var settingsProxy = Bootstrapper.Container.Resolve<IProxyBase<Setting, string>>();

            var settings = await settingsProxy.GetAsync();

            Assert.True(settings.Count > 0);
        }
    }
}
