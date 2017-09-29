using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class TestProxy : ITestProxy
    {
        private readonly ILogger _logger;
        private readonly IPhpCrudApiService _phpCrudApiService;
        
        public TestProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
        {
            _logger = logger;
            _phpCrudApiService = phpCrudApiService;
        }
        public async Task<List<Test>> GetTestsAsync()
        {
            var tests = (await _phpCrudApiService.GetDataAsync<TestWrapper>("tblTest")).tblTest;

            //var withoutTransform = await _phpCrudApiService.GetDataAsync("tblTest", false);

            return tests;
        }

        public async Task<Test> GetTestAsync(int id)
        {
            var test = await _phpCrudApiService.GetDataAsync<Test>("tblTest");
            //https://www.aikido-forchheim.de/database/117ee320a66446c6a8c950cbcf0b89b8/api.php/tblTest?csrf=291995873&filter=ID,eq,-1&transform=1
            return test;
        }

        public async Task UpdateUserAsync(Test test)
        {
            await _phpCrudApiService.UpdateDataAsync("tblTest" + "/" + test.ID, test);
        }
    }
}
