using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Proxies;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IRepositoryBase<T, in TId>
    {
        Task<int> CreateAsync(T obj);
        
        Task<List<T>> GetAsync();

        Task<T> GetAsync(TId id);

        Task<List<T>> GetAsync(List<Filter> filters);

        Task<int> UpdateAsync(T obj);

        Task<int> DeleteAsync(T obj);
    }
}
