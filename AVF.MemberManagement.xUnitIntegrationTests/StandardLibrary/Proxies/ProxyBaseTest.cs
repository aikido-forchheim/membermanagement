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
        public async void CreateDefaultTestObjectShouldFail()
        {
            var testBaseProxy = new ProxyBase<TblTestBase, TestObject, string>(_fakeLogger, _phpCrudApiService);

            var testObject = new TestObject();

            await Assert.ThrowsAsync<IOException>(async () =>
            {
                await testBaseProxy.CreateAsync(testObject);
            });
        }

        [Fact]
        public async void CreateTest()
        {
            var testBaseProxy = new ProxyBase<TblTestBase, TestObject, string>(_fakeLogger, _phpCrudApiService);

            var id = Guid.NewGuid().ToString();

            var testObject = new TestObject { Id = id };

            var createResult = await testBaseProxy.CreateAsync(testObject);

            Assert.True(createResult == 0);

            //TODO: Delete generated TestObject to preserve space in db
        }

        [Fact]
        public async void ReadTest()
        {
            var testBaseProxy = new ProxyBase<TblTestBase, TestObject, string>(_fakeLogger, _phpCrudApiService);

            var testBaseObjects = await testBaseProxy.GetAsync();

            Assert.True(testBaseObjects != null);
        }

    }
}
