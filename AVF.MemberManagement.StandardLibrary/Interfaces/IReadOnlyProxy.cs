using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Proxies;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IReadOnlyProxy<T, in TId>
    {
        Task<List<T>> GetAsync();
        Task<T> GetAsync(TId id);

        Task<List<T>> FilterAsync(List<Filter> filters);

        Task<TablePropertiesBase> GetTablePropertiesAsync();
    }
}
