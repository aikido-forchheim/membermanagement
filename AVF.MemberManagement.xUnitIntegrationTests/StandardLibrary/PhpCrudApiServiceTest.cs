using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tables;
using IT2media.Extensions.Object;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class PhpCrudApiServiceTest : TestBase
    {
        [Fact]
        public async void GetObjectWithoutWrapper()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            var settingsWrapper = await phpCrudApiService.GetDataAsync<TblSettings>("Settings");

            var file = settingsWrapper.DumpToFile("SettingsWrapper");
        }
    }
}
