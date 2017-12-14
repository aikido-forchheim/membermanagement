using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Options;
using AVF.MemberManagement.StandardLibrary.Proxies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AVF.MemberManagement.Proxies
{
    public class FileProxy<TTbl, T> : FileProxyBase<TTbl, T, int>, IProxy<T>
        where TTbl : ITable<T>, new()
        where T : IIntId
    {
        public FileProxy(IOptions<FileProxyOptions> fileProxyOptions) : base(fileProxyOptions)
        {
        }

        public Task<string> UpdateAsync(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
