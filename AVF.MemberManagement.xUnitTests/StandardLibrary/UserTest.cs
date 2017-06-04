using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;
using Xunit;
using Xunit.Sdk;

namespace AVF.MemberManagement.xUnitTests.StandardLibrary
{
    public class UserTest
    {
        [Fact()]
        public void TestUserBaseForNull()
        {
            var user = new User
            {
                Username = "Username",
                Password = "Password",
                Active = true,
                UserId = 666
            };

            var userBase = user;

            Assert.NotNull(userBase);
        }
    }
}
