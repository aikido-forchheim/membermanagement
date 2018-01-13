using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class Proxy<TTbl, T> : ProxyBase<TTbl, T, int>, IProxy<T> 
        where TTbl : ITable<T>, new()
        where T : IIntId
    {
        public Proxy(ILogger logger, IPhpCrudApiService phpCrudApiService) : base(logger, phpCrudApiService)
        {
        }
    }
}
