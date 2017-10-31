using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class ProxyBase<TTbl, T, TId> : IProxyBase<T, TId> where TTbl : ITable<T>, new()
        where T : IId<TId>
    {
        private readonly string _uri;

        private readonly ILogger _logger;
        public readonly IPhpCrudApiService _phpCrudApiService;

        public ProxyBase(ILogger logger, IPhpCrudApiService phpCrudApiService)
        {
            _uri = new TTbl().Uri;

            _logger = logger;
            _phpCrudApiService = phpCrudApiService;
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
