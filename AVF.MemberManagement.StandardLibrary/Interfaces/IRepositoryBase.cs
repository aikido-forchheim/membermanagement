using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IRepositoryBase<T, in TId>
    {
        Task<int> CreateAsync(T obj);
        
        Task<List<T>> GetAsync();

        Task<T> GetAsync(TId id);

        Task<int> UpdateAsync(T obj);

        Task<int> DeleteAsync(T obj);
    }
}
