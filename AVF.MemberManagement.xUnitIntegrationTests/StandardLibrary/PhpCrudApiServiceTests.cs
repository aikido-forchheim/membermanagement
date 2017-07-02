using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using IT2media.Extensions.Object;
using Xunit;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class PhpCrudApiServiceTests : TestBase
    {
        [Fact]
        public async void GetObjectWithoutWrapper()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            var settingsWrapper = await phpCrudApiService.GetDataAsync<SettingsWrapper>("Settings");

            settingsWrapper.DumpToFile("SettingsWrapper");
        }
    }
}
