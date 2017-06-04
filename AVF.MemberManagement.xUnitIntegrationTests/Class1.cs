using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class Class1
    {
        //[Fact()]
        //public void WriteIntegrationTestSettings()
        //{
        //    var sampleSettings = new IntegrationTestSettings();
        //    var sampleSettingsToWrite = JsonConvert.SerializeObject(sampleSettings);

        //    File.WriteAllText("C:\\temp\\IntegrationTestSettings.json", sampleSettingsToWrite, Encoding.UTF8);
        //}

        [Fact()]
        public void StartBasicTest()
        {
            bool couldLogin;

            var integrationTestSettings = IntegrationTestSettings.Get();

            couldLogin = integrationTestSettings.RestApiAccount.Username != "username";

            Assert.True(couldLogin);
        }
    }
}
