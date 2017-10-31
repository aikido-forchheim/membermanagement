using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class ProxyBase<TTbl, T, TId> : IProxyBase<T, TId> where TTbl : ITable<T>
    {
        private readonly IPhpCrudApiService _phpCrudApiService;

        private readonly string _uri;

        public ProxyBase(IPhpCrudApiService phpCrudApiService, string uri)
        {
            _phpCrudApiService = phpCrudApiService;
            _uri = uri;
        }

        public async Task<List<T>> GetAsync()
        {
            var table = await _phpCrudApiService.GetDataAsync<TTbl>(_uri);

            var tableData = table.Rows;

            return tableData;
        }

        public async Task<T> GetAsync(TId id)
        {
            var obj = await _phpCrudApiService.GetDataAsync<T>($"{_uri}/{id}");

            return obj;
        }

        public async Task<string> UpdateAsync(T obj, TId id)
        {
            return await _phpCrudApiService.UpdateDataAsync($"{_uri}/{id}", obj);
        }
    }
}
