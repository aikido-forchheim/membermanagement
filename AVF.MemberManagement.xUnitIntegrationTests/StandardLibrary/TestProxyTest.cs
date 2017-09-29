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

            var testValues = await testProxy.GetTestEntriesAsync();

            Assert.True(testValues.Count > 2);
        }

        [Fact]
        public async Task CanGetSingleMinusValues()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testValue = await testProxy.GetTestObjectAsync(-1);

            Assert.True(testValue.ID == -1);
        }

        [Fact]
        public async Task UpdateTestZero()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testObject = new Test
            {
                ID = 0,
                Text = "ÖÄÜ"
            };

            var updateResult = await testProxy.UpdateTestObjectAsync(testObject);

            Assert.True(updateResult == "null");
        }

        [Fact]
        public async Task UpdateTestMinus()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testObject = new Test
            {
                ID = -1,
                Text = "ok"
            };

            var updateResult = await testProxy.UpdateTestObjectAsync(testObject);

            Assert.True(updateResult == "1");
        }
    }
}
