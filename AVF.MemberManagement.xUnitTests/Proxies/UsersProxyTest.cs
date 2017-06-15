using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AVF.MemberManagement.xUnitTests.Proxies
{
    public class UsersProxyTest
    {
        [Fact]
        public async Task NotFoundUserShouldNotBeNull()
        {
            IUsersProxy usersProxy = new UsersProxy(A.Fake<ILogger>(), A.Fake<IPhpCrudApiService>());

            var user = await usersProxy.GetUserAsync("");

            Assert.NotNull(user);
        }
    }
}
