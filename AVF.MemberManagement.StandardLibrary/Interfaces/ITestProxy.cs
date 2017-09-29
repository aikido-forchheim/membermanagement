using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ITestProxy
    {
        Task<List<Test>> GetTestEntriesAsync();

        Task<Test> GetTestObjectAsync(int id);

        Task<string> UpdateTestObjectAsync(Test test);
    }
}