using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class IntegrationTestSettings
    {
        public RestApiAccount RestApiAccount { get; set; } = new RestApiAccount() { Username = "username", Password = "password", ApiUrl = "https://www.homepage.com/api.php"};
    }
}
