using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;
using Newtonsoft.Json;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class IntegrationTestSettings
    {
        public RestApiAccount RestApiAccount { get; set; } = new RestApiAccount { Username = "username", Password = "password", ApiUrl = "https://www.homepage.com/api.php"};

        public User User { get; set; } = new User {Username = "username", Password = "password"};
        
        public static IntegrationTestSettings Get()
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var pathFromDocuments = Path.Combine(home,"Documents/IntegrationTestSettings.json");
            
            var pathToIntegrationTestSettings = (Environment.GetEnvironmentVariable("AVF.MemberManagement.xUnitIntegrationTests.IntegrationTestSettings") ?? Environment.GetEnvironmentVariable("ITS")) ??
                                             pathFromDocuments;

            IntegrationTestSettings integrationTestSettings;

            if (File.Exists(pathToIntegrationTestSettings))
            {
                var json = File.ReadAllText(pathToIntegrationTestSettings, Encoding.UTF8);

                integrationTestSettings = JsonConvert.DeserializeObject<IntegrationTestSettings>(json);
            }
            else
            {
                throw new Exception("Please set environment variable with path to settings: AVF.MemberManagement.xUnitIntegrationTests.IntegrationTestSettings");
            }

            return integrationTestSettings;
        }
    }
}
