using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IProxyBase<T, in TId> : IReadOnlyProxy<T, TId>
    {
        Task<int> CreateAsync(T obj);

        //Read is definded in ReadOnlyProxy

        Task<int> UpdateAsync(T obj);

        //Task<int> DeleteAsync(T obj);
        //Task<int> DeleteAsync(TId id);
    }
}