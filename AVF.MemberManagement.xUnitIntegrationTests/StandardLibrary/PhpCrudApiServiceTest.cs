using System;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using IT2media.Extensions.Object;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class PhpCrudApiServiceTest : TestBase
    {
        [Fact]
        public async void CreateTest()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();

            var tblTestsWrapper = await phpCrudApiService.GetDataAsync<TblTests>("tblTest");

            var list = tblTestsWrapper.Rows;

            var nextId = list.Max(o => o.Id) + 1;

            var dataObject = new Test { Id = nextId, Text = Guid.NewGuid().ToString() };

            var insertResult = await phpCrudApiService.SendDataAsync("tblTest", dataObject);

            Assert.True(insertResult != null);
        }

        [Fact]
        public async void ReadTest()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();

            var settingsWrapper = await phpCrudApiService.GetDataAsync<TblSettings>("Settings");

            Assert.True(settingsWrapper.Rows != null && settingsWrapper.Rows.Count > 0);
        }
    }
}
