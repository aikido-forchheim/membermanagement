using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Repositories
{
    public class TestsRepository
    {
        private readonly ITestProxy _testProxy;

        public TestsRepository(ITestProxy testProxy)
        {
            _testProxy = testProxy;
        }

        public async Task<List<Test>> GetAsync()
        {
            return await _testProxy.GetAsync();
        }

        public async Task<Test> GetAsync(int id)
        {
            return await _testProxy.GetAsync(id);
        }
    }
}
