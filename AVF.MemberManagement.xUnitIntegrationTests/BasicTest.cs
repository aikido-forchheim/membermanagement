using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Practices.Unity;
using Xunit;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Prism.Ioc;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class BasicTest : TestBase
    {
        //private const string PathToIntegrationTestSettingsJson = "C:\\temp\\IntegrationTestSettings.json";

        //[Fact()]
        //public void WriteIntegrationTestSettings()
        //{
        //    var sampleSettings = new IntegrationTestSettings();
        //    var sampleSettingsToWrite = JsonConvert.SerializeObject(sampleSettings);

        //    File.WriteAllText(PathToIntegrationTestSettingsJson, sampleSettingsToWrite, Encoding.UTF8);

        //    Process.Start(PathToIntegrationTestSettingsJson);
        //}

        [Fact]
        public void CouldIntegrationTestSettingsBeLoaded()
        {
            var couldIntegrationTestSettingsBeLoaded = Bootstrapper.Container.Resolve<IntegrationTestSettings>().RestApiAccount.Username != "username";

            Assert.True(couldIntegrationTestSettingsBeLoaded);
        }

        [Fact]
        public async Task TestPhpCrudApiService()
        {
            var usersProxy = Bootstrapper.Container.Resolve<IProxy<User>>();

            var users = await usersProxy.GetAsync();

            var couldReadUsers = users.Count > 0;

            Assert.True(couldReadUsers);
        }

        [Fact]
        public async Task VerifyPassword()
        {
            var usersProxy = Bootstrapper.Container.Resolve<IRepository<User>>();

            var localUser = Bootstrapper.Container.Resolve<IntegrationTestSettings>().User;

            var serverUser = (await usersProxy.GetAsync()).Single(user => user.Username == localUser.Username);

            var passwordService = Bootstrapper.Container.Resolve<IPasswordService>();

            var isPasswordValid = await passwordService.IsValidAsync(localUser.Password, serverUser.Password, AppId);

            Assert.True(isPasswordValid);
        }
    }
}
