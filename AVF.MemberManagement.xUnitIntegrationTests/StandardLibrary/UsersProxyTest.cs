using System;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using Prism.Ioc;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class UsersProxyTest : TestBase
    {
        [Fact]
        public async Task GetUserAsyncTest()
        {
            var usersProxy = Bootstrapper.Container.Resolve<IRepository<User>>();

            var userName = Bootstrapper.Container.Resolve<IntegrationTestSettings>().User.Username;

            var serverUser = (await usersProxy.GetAsync()).Single(user => user.Username == userName);

            Assert.True(serverUser != null && serverUser.Id > 0);
        }

        [Fact]
        public async Task GetUserAsyncWithNullShouldThrow()
        {
            var usersProxy = Bootstrapper.Container.Resolve<IRepository<User>>();

            await Assert.ThrowsAsync<InvalidOperationException>(async () => (await usersProxy.GetAsync()).Single(user => user.Username == null));
        }
    }
}
