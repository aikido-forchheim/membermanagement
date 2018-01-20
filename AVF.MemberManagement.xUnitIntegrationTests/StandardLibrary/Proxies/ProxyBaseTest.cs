using System;
using System.IO;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary.Proxies
{
    public class ProxyBaseTest : TestBase
    {
        private readonly IPhpCrudApiService _phpCrudApiService;
        private readonly ILogger _fakeLogger;

        public ProxyBaseTest()
        {
            _phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            _fakeLogger = A.Fake<ILogger>();
        }

        [Fact]
        public async Task CreateDefaultTestObjectShouldFail()
        {
            var testBaseProxy = new ProxyBase<TblTestBase, TestObject, string>(_fakeLogger, _phpCrudApiService);

            var testObject = new TestObject();

            await Assert.ThrowsAsync<IOException>(async () =>
            {
                await testBaseProxy.CreateAsync(testObject);
            });
        }

        [Fact]
        public async Task CreateTest()
        {
            await CreateAndDelete();
        }

        [Fact]
        public async Task ReadTest()
        {
            var testBaseProxy = new ProxyBase<TblTestBase, TestObject, string>(_fakeLogger, _phpCrudApiService);

            var testBaseObjects = await testBaseProxy.GetAsync();

            Assert.True(testBaseObjects != null);
        }

        [Fact]
        public async Task DeleteTest()
        {
            await CreateAndDelete();
        }

        [Fact]
        public async Task GetTablePropertiesTest()
        {
            var settingsProxy = new ProxyBase<TblSettings, Setting, string>(_fakeLogger, _phpCrudApiService);

            var settings = await settingsProxy.GetAsync();

            var tableProperties = await settingsProxy.GetTablePropertiesAsync();

            Assert.True(settings.Count() == tableProperties.RowCount);
        }


        private async Task CreateAndDelete()
        {
            var testBaseProxy = new ProxyBase<TblTestBase, TestObject, string>(_fakeLogger, _phpCrudApiService);

            var id = Guid.NewGuid().ToString();

            var testObject = new TestObject { Id = id };

            var createResult = await testBaseProxy.CreateAsync(testObject);

            Assert.True(createResult == 0);

            var deleteResult = await testBaseProxy.DeleteAsync(id);

            Assert.True(deleteResult == 1);
        }
    }
}
