using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tables;
using AVF.MemberManagement.StandardLibrary.Proxies;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;

namespace AVF.MemberManagement.xUnitTests.StandardLibrary
{
    public class SettingsProxyTest
    {
        [Fact]
        public async void LoadSettingsCacheAsyncTest()
        {
            var fakePhpCrudApiService = GetFakePhpCrudApiService();

            ISettingsProxy settingsProxy = new SettingsProxy(A.Fake<ILogger>(), fakePhpCrudApiService);
            var settingsCache = await settingsProxy.LoadSettingsCacheAsync();

            Assert.NotNull(settingsCache);
        }

        private static IPhpCrudApiService GetFakePhpCrudApiService()
        {
            var fakePhpCrudApiService = A.Fake<IPhpCrudApiService>();

            A.CallTo(() => fakePhpCrudApiService.GetDataAsync<TblSettings>(A<string>.Ignored))
                .Returns(Task.FromResult(JsonConvert.DeserializeObject<TblSettings>(
                    "{\"Settings\":[{\"Key\":\"HashAlgorithm\",\"Value\":\"SHA512\",\"Des\":\"HashAlgorithm used for password hashing\"},{\"Key\":\"Logo\",\"Value\":\"https://raw.githubusercontent.com/aikido-forchheim/assets/master/logo1024.jpg\",\"Des\":\"Vereinslogo\"}]}")));
            return fakePhpCrudApiService;
        }

        [Fact]
        public async void AssertNotExistingSettingReturnsNull()
        {
            var fakePhpCrudApiService = GetFakePhpCrudApiService();

            ISettingsProxy settingsProxy = new SettingsProxy(A.Fake<ILogger>(), fakePhpCrudApiService);

            var notExistingSetting = await settingsProxy.GetSettingAsync("SomeNotExistingKey");

            Assert.Null(notExistingSetting);
        }

        [Fact]
        public async void AssertNotExistingSettingWithDefaultReturnsDefault()
        {
            var fakePhpCrudApiService = GetFakePhpCrudApiService();

            ISettingsProxy settingsProxy = new SettingsProxy(A.Fake<ILogger>(), fakePhpCrudApiService);

            const string defaultValue = "IamDefault";
            var notExistingSetting = await settingsProxy.GetSettingAsync("SomeNotExistingKey", defaultValue);

            Assert.True(notExistingSetting.Value == defaultValue);
        }

    }
}
