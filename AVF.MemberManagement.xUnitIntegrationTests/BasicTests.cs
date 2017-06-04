using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class BasicTests
    {
        //[Fact()]
        //public void WriteIntegrationTestSettings()
        //{
        //    var sampleSettings = new IntegrationTestSettings();
        //    var sampleSettingsToWrite = JsonConvert.SerializeObject(sampleSettings);

        //    File.WriteAllText("C:\\temp\\IntegrationTestSettings.json", sampleSettingsToWrite, Encoding.UTF8);
        //}

        [Fact]
        public void CouldIntegrationTestSettingsBeLoaded()
        {
            var integrationTestSettings = IntegrationTestSettings.Get();

            var couldIntegrationTestSettingsBeLoaded = integrationTestSettings.RestApiAccount.Username != "username";

            Assert.True(couldIntegrationTestSettingsBeLoaded);
        }

        [Fact]
        public async void TestPhpCrudApiService()
        {
            var integrationTestSettings = IntegrationTestSettings.Get();

            var logger = A.Fake<ILogger>();

            IAccountService accountService = new AccountService(logger);
            accountService.Init(integrationTestSettings.RestApiAccount);

            IPhpCrudApiService phpCrudApiService = new PhpCrudApiService(logger, accountService);

            IUsersProxy usersProxy = new UsersProxy(logger, phpCrudApiService);

            var users = await usersProxy.GetUsersAsync();

            var couldReadUsers = users.Count > 0;

            Assert.True(couldReadUsers);
        }
    }
}
