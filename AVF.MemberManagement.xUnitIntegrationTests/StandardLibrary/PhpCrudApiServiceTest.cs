using System;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests.StandardLibrary
{
    public class PhpCrudApiServiceTest : TestBase
    {
        [Fact]
        public async void CreateTest()
        {
            var insertResult = await CreateNewObjectInTblTest();

            Assert.True(insertResult != null);
        }

        private async System.Threading.Tasks.Task<string> CreateNewObjectInTblTest()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();

            var tblTestsWrapper = await phpCrudApiService.GetDataAsync<TblTests>("tblTest");

            var list = tblTestsWrapper.Rows;

            var nextId = list.Max(o => o.Id) + 1;

            var dataObject = new Test { Id = nextId, Text = Guid.NewGuid().ToString() };

            var insertResult = await phpCrudApiService.SendDataAsync("tblTest", dataObject);

            return insertResult;
        }

        [Fact]
        public async void ReadTest()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();

            var settingsWrapper = await phpCrudApiService.GetDataAsync<TblSettings>("Settings");

            Assert.True(settingsWrapper.Rows != null && settingsWrapper.Rows.Count > 0);
        }

        [Fact]
        public async void UpdateTest()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();

            var tblTestsWrapper = await phpCrudApiService.GetDataAsync<TblTests>("tblTest");

            var lastRow = tblTestsWrapper.Rows.Last();

            var textForUpdate = Guid.NewGuid().ToString();

            lastRow.Text = textForUpdate;

            //SEND URI WITH /ID !!!
            var updateResult = await phpCrudApiService.UpdateDataAsync($"tblTest/{lastRow.Id}", lastRow);

            Assert.True(updateResult != null && updateResult != "null");

            var tblTestsWrapperControl = await phpCrudApiService.GetDataAsync<TblTests>("tblTest");
            
            var lastRowControl = tblTestsWrapperControl.Rows.Last();

            Assert.True(lastRowControl.Text == textForUpdate);
        }

        [Fact]
        public async void DeleteTest()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();

            var initialRowsCount = (await phpCrudApiService.GetDataAsync<TblTests>("tblTest")).Rows.Count;

            var createResult = await CreateNewObjectInTblTest();
            var wasCreateSuccessful = createResult != null;

            Assert.True(wasCreateSuccessful);
            
            var tblTestsWrapper = await phpCrudApiService.GetDataAsync<TblTests>("tblTest");
            
            var lastRow = tblTestsWrapper.Rows.Last();

            var result = await phpCrudApiService.DeleteDataAsync($"tblTest/{lastRow.Id}");

            Assert.True(result != null && result != "null" && result == "1");

            var rowsCountAfterInsertAndDelete = (await phpCrudApiService.GetDataAsync<TblTests>("tblTest")).Rows.Count;

            Assert.True(rowsCountAfterInsertAndDelete == initialRowsCount);
        }
    }
}
