using AVF.MemberManagement.StandardLibrary.Models.Tbo;
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
                Id = 666
            };

            var userBase = user;

            Assert.NotNull(userBase);
        }
    }
}
