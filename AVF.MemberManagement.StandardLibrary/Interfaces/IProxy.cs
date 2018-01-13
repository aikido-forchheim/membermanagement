using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IProxy<T> : IProxyBase<T, int> where T : IIntId
    {
    }
}