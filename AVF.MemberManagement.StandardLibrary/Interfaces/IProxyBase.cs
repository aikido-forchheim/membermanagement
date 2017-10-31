using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IProxyBase<T, in TId> : IReadOnlyProxy<T, TId>
    {
        Task<string> UpdateAsync(T obj, TId id);
    }
}