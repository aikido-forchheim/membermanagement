using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tables;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class ProxyBaseInt<TTbl, T> : ProxyBase<TTbl, T, int>, IProxyBaseInt<T, int> where TTbl : ITable<T>
        where T : IIntId
    {
        public ProxyBaseInt(IPhpCrudApiService phpCrudApiService, string uri) : base(phpCrudApiService, uri)
        {
        }

        public Task<string> UpdateAsync(T obj)
        {
            return UpdateAsync(obj, obj.Id);
        }
    }
}
