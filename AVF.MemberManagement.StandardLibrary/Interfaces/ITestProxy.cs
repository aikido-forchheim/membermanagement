using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ITestProxy
    {
        Task<List<Test>> GetTestsAsync();

        Task<Test> GetTestAsync(int id);

        Task UpdateUserAsync(Test test);
    }
}