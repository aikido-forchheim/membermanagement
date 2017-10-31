using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;
using Xunit;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class TestsProxyTest : TestBase
    {
        [Fact]
        public async Task GetAllTestEntriesInTable()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testValues = await testProxy.GetAsync();

            Assert.True(testValues.Count > 0);
        }

        [Fact]
        public async Task CanGetMinusValues()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testValues = await testProxy.GetAsync();

            Assert.True(testValues.Count > 2);
        }

        [Fact]
        public async Task CanGetSingleMinusValues()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testValue = await testProxy.GetAsync(-1);

            Assert.True(testValue.Id == -1);
        }

        [Fact]
        public async Task UpdateTestZero()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testObject = new Test
            {
                Id = 0,
                Text = "ÖÄÜ"
            };

            var updateResult = await testProxy.UpdateAsync(testObject);

            Assert.True(updateResult == "null");
        }

        [Fact]
        public async Task UpdateTestMinus()
        {
            var testProxy = Bootstrapper.Container.Resolve<ITestProxy>();

            var testObject = new Test
            {
                Id = -1,
                Text = "ok"
            };

            var updateResult = await testProxy.UpdateAsync(testObject);

            Assert.True(updateResult == "1");
        }
    }
}
