using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;

namespace AVF.MemberManagement.Proxies
{
    public class FileProxyBase<TTbl, T, TId> : IProxyBase<T, TId> where TTbl : ITable<T>, new()
        where T : IId<TId>
    {
        public Task<List<T>> GetAsync()
        {
            Debug.WriteLine(new TTbl().Uri);

            throw new NotImplementedException();
        }

        public Task<T> GetAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateAsync(T obj, TId id)
        {
            throw new NotImplementedException();
        }
    }
}
