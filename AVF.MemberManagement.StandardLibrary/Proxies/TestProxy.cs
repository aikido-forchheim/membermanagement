using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class TestProxy : ITestProxy
    {
        private const string Uri = "tblTest";

        private readonly IPhpCrudApiService _phpCrudApiService;
        
        public TestProxy(IPhpCrudApiService phpCrudApiService)
        {
            _phpCrudApiService = phpCrudApiService;
        }

        public async Task<List<Test>> GetTestsAsync()
        {
            var testTableEntries = (await _phpCrudApiService.GetDataAsync<TestWrapper>(Uri)).tblTest;

            return testTableEntries;
        }

        public async Task<Test> GetTestAsync(int id)
        {
            var testObject = await _phpCrudApiService.GetDataAsync<Test>($"{Uri}/{id}");

            return testObject;
        }

        public async Task UpdateUserAsync(Test test)
        {
            await _phpCrudApiService.UpdateDataAsync($"{Uri}/{test.ID}", test);
        }
    }
}
