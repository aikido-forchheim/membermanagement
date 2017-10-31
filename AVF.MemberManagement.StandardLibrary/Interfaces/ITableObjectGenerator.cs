using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ITableObjectGenerator
    {
        Task<Dictionary<string, string>> GetColumnTypes(IPhpCrudApiService phpCrudApiService, string tablename);
        Task<List<string>> GetTableNames(IPhpCrudApiService phpCrudApiService);
    }
}