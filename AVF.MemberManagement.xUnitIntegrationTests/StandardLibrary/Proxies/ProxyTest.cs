using System.IO;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Proxies
{
    public class ProxyTest : TestBase
    {
        private readonly IPhpCrudApiService _phpCrudApiService;
        private readonly ILogger _fakeLogger;

        public ProxyTest()
        {
            _phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            _fakeLogger = A.Fake<ILogger>();
        }

        [Fact]
        public async Task CreateTestShouldFail()
        {   
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            var fakeLogger = A.Fake<ILogger>();

            var testProxy = new Proxy<TblTests, Test>(fakeLogger, phpCrudApiService);

            //test object with default id 0 should fail
            var test = new Test();

            await Assert.ThrowsAsync<IOException>(async () =>
            {
                await testProxy.CreateAsync(test);
            });
        }

        [Fact]
        public async Task GetTablePropertiesTest()
        {
            var mitgliederProxy = new Proxy<TblMitglieder, Mitglied>(_fakeLogger, _phpCrudApiService);

            var mitglieder = await mitgliederProxy.GetAsync();

            var tableProperties = await mitgliederProxy.GetTablePropertiesAsync();

            Assert.True(mitglieder.Count == tableProperties.RowCount);
        }
    }
}
