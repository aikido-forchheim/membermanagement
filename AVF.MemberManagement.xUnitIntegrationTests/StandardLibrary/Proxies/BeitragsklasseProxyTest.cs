using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Proxies
{
    public class BeitragsklasseProxyTest : TestBase
    {
        [Fact]
        public async void CanGetBeitragsklasse()
        {
            var proxy = Bootstrapper.Container.Resolve<IProxy<Beitragsklasse>>();

            var beitragsklassen = await proxy.GetAsync();

            Assert.True(beitragsklassen.Count > 0);
        }

        [Fact]
        public async void CanGetSingleBeitragsklasse()
        {
            var proxy = Bootstrapper.Container.Resolve<IProxy<Beitragsklasse>>();

            var beitragsklasse = await proxy.GetAsync(1);

            Assert.True(beitragsklasse.BeitragsklasseRomanNumeral == RomanNumeral.I);
        }
    }
}
