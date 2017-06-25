using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class UsersProxyTest : TestBase
    {
        [Fact]
        public async Task GetUserAsyncTest()
        {
            var usersProxy = Bootstrapper.Container.Resolve<IUsersProxy>();

            var userName = Bootstrapper.Container.Resolve<IntegrationTestSettings>().User.Username;

            var serverUser = await usersProxy.GetUserAsync(userName);

            Assert.True(serverUser != null && serverUser.UserId > 0);
        }

        [Fact]
        public async Task GetUserAsyncIsNotNullTest()
        {
            var usersProxy = Bootstrapper.Container.Resolve<IUsersProxy>();

            var serverUser = await usersProxy.GetUserAsync(null);

            Assert.True(serverUser != null);
        }
    }
}
