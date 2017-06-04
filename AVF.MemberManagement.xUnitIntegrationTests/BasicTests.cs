using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.PortableLibrary.Services;
using AVF.MemberManagement.Services;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Services;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class BasicTests : TestBase
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
            var usersProxy = Bootstrapper.Container.Resolve<IUsersProxy>();

            var users = await usersProxy.GetUsersAsync();

            var couldReadUsers = users.Count > 0;

            Assert.True(couldReadUsers);
        }

        [Fact]
        public async Task VerfiyPassword()
        {
            var usersProxy = Bootstrapper.Container.Resolve<IUsersProxy>();

            var localUser = Bootstrapper.Container.Resolve<IntegrationTestSettings>().User;

            var serverUser = await usersProxy.GetUserAsync(localUser.Username);
            
            var passwordService = Bootstrapper.Container.Resolve<IPasswordService>();

            var isPasswordValid = await passwordService.IsValidAsync(localUser.Password, serverUser.Password, AppId);

            Assert.True(isPasswordValid);
        }
    }
}
