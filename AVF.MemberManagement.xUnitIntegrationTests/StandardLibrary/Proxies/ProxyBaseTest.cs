using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Services;
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
        [Fact]
        public async void CreateTestShouldFail()
        {
            //THIS SHOULD BE A TEST FOR ProxyTest with IIntId, ProxyBase with string as id for example should fail on null and string.empty
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            var fakeLogger = A.Fake<ILogger>();

            var testProxy = new ProxyBase<TblTests, Test, int>(fakeLogger, phpCrudApiService);

            //test object with default id 0 should fail
            var testObject = new Test();

            var createResult = await testProxy.CreateAsync(testObject);

            Assert.True(createResult == 1);
        }
    }
}
