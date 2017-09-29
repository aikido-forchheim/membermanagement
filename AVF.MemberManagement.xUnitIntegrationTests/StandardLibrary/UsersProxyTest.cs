using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
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
        public async Task GetUserAsyncWithNullShouldThrow()
        {
            var usersProxy = Bootstrapper.Container.Resolve<IUsersProxy>();

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await usersProxy.GetUserAsync(null));
        }
    }
}
