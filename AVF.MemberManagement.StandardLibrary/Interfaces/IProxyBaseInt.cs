using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IProxyBaseInt<T, in TId> : IProxyBase<T, TId> where T : IIntId
    {
        Task<string> UpdateAsync(T obj);
    }
}