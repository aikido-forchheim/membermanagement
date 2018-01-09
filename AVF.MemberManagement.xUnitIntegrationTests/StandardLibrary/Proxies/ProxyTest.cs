using System.IO;
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
        [Fact]
        public async void CreateTestShouldFail()
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
    }
}
