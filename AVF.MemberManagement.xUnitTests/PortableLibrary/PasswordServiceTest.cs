using System.Threading.Tasks;
using AVF.MemberManagement.PortableLibrary.Services;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using FakeItEasy;
using Xunit;

namespace AVF.MemberManagement.xUnitTests.PortableLibrary
{
    public class PasswordServiceTest
    {
        private const string Password = "abc";
        private const string Pepper = "pepper";

        [Fact]
        public async void HashPasswordTest()
        {
            var passwordService = GetPasswordService();

            var hash = await passwordService.HashPasswordAsync(Password, Pepper);

            Assert.True(hash.Length > 20);
        }

        [Fact]
        public async void IsValidTest()
        {
            var passwordService = GetPasswordService();

            var hash = await passwordService.HashPasswordAsync(Password, Pepper);

            var isValid = await passwordService.IsValidAsync(Password, hash, Pepper);

            Assert.True(isValid);
        }

        private static PasswordService GetPasswordService()
        {
            var fakeSettingsRepository = A.Fake<IRepositoryBase<Setting, string>>();

            A.CallTo(() => fakeSettingsRepository.GetAsync(A<string>.Ignored)).Returns(Task.FromResult(new Setting { Id = "HashAlgorithm", Value = "SHA256" }));

            var passwordService = new PasswordService(fakeSettingsRepository);

            return passwordService;
        }
    }
}
