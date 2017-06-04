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
    }
}
