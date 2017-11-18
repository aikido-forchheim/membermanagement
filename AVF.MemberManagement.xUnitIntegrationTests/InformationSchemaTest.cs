using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Practices.Unity;
using Xunit;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class InformationSchemaTest : TestBase
    {
        [Fact]
        public async void CountTablesViaInformationSchemaTest()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            var tableObjectGenerator = Bootstrapper.Container.Resolve<ITableObjectGenerator>();

            var tablesAndViewList = await tableObjectGenerator.GetTableNames(phpCrudApiService);

            var viewNames = new[] { "tables", "Test" };

            var tableNames = tablesAndViewList.Where(t => !viewNames.Contains(t)).ToList();

            Assert.True(tableNames.Count == 18);
        }

        [Fact]
        public async void ReadTableColumnsTest()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            var tableObjectGenerator = Bootstrapper.Container.Resolve<ITableObjectGenerator>();
            await ProcessTable(phpCrudApiService, tableObjectGenerator, "tblMitglieder");
        }

        [Fact]
        public async void ProcessAllTables()
        {
            var phpCrudApiService = Bootstrapper.Container.Resolve<IPhpCrudApiService>();
            var tableObjectGenerator = Bootstrapper.Container.Resolve<ITableObjectGenerator>();

            var tables = await tableObjectGenerator.GetTableNames(phpCrudApiService);

            foreach (var table in tables)
            {
                await ProcessTable(phpCrudApiService, tableObjectGenerator, table);
            }
        }

        private static async Task ProcessTable(IPhpCrudApiService phpCrudApiService, ITableObjectGenerator tableObjectGenerator, string tablename)
        {
            var columnTypes = await tableObjectGenerator.GetColumnTypes(phpCrudApiService, tablename);

            foreach (var columnType in columnTypes)
            {
                Assert.True(columnType.Value != null);
            }
        }
    }
}
