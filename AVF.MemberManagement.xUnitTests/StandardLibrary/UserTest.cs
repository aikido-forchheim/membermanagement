using AVF.MemberManagement.StandardLibrary.Models;
using Xunit;

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
