using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.Proxies
{
    public class FileProxy<TTbl, T> : FileProxyBase<TTbl, T, int>, IProxy<T>
        where TTbl : ITable<T>, new()
        where T : IIntId
    {
        public Task<string> UpdateAsync(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
