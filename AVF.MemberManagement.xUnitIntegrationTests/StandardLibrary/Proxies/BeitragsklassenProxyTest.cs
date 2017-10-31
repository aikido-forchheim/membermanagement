using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Xunit;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Proxies
{
    public class BeitragsklassenProxyTest : TestBase
    {
        [Fact]
        public async void CanGetBeitragsklasse()
        {
            var proxy = Bootstrapper.Container.Resolve<IBeitragsklassenProxy>();

            var beitragsklassen = await proxy.GetAsync();

            Assert.True(beitragsklassen.Count > 0);
        }

        [Fact]
        public async void CanGetSingleBeitragsklasse()
        {
            var proxy = Bootstrapper.Container.Resolve<IBeitragsklassenProxy>();

            var beitragsklasse = await proxy.GetAsync(1);

            Assert.True(beitragsklasse.BeitragsklasseRomanNumeral == RomanNumeral.I);
        }
    }
}
