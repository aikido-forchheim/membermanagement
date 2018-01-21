using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IProxy<T> : IProxyBase<T, int> where T : IIntId
    {
        new Task<TableProperties> GetTablePropertiesAsync();
    }
}