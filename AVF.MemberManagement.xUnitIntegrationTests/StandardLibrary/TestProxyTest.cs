using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Xunit;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class TestProxyTest : TestBase
    {
        [Fact]
        public async Task CanGetMinusValues()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testValues = await testProxy.GetTestsAsync();

            Assert.True(testValues.Count > 2);
        }

        [Fact]
        public async Task CanGetSingleMinusValues()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testValue = await testProxy.GetTestAsync(-1);

            Assert.True(testValue.ID == -1);
        }

        [Fact]
        public async Task UpdateTestMinus()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var test = new Test
            {
                ID = 0,
                Text = "ÖÄÜ"
            };

            await testProxy.UpdateUserAsync(test);
        }
    }
}
