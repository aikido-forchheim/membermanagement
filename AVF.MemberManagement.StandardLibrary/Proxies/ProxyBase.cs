using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class ProxyBase<TTbl, T, TId> : IProxyBase<T, TId> where TTbl : ITable<T>, new()
        where T : IId<TId>, new()
    {
        private readonly string _uri;

        private readonly ILogger _logger;
        private readonly IPhpCrudApiService _phpCrudApiService;

        public ProxyBase(ILogger logger, IPhpCrudApiService phpCrudApiService)
        {
            _uri = new TTbl().Uri;

            _logger = logger;
            _phpCrudApiService = phpCrudApiService;
        }

        #region Create

        public async Task<int> CreateAsync(T obj)
        {
            var insertResult = await _phpCrudApiService.SendDataAsync(_uri, obj);

            return int.Parse(insertResult);
        }

        #endregion

        #region Read

        public async Task<List<T>> GetAsync()
        {
            var table = await _phpCrudApiService.GetDataAsync<TTbl>(_uri);

            return table.Rows;
        }

        public async Task<T> GetAsync(TId id)
        {
            var obj = await _phpCrudApiService.GetDataAsync<T>($"{_uri}/{id}");

            return obj;
        }

        #endregion

        #region Update

        public async Task<int> UpdateAsync(T obj)
        {
            var updateResult = await _phpCrudApiService.UpdateDataAsync($"{_uri}/{obj.Id}", obj);

            return int.Parse(updateResult);
        }

        #endregion

        #region Delete

        public async Task<int> DeleteAsync(T obj) => await DeleteAsync(obj.Id);

        public async Task<int> DeleteAsync(TId id)
        {
            var deleteResult = await _phpCrudApiService.DeleteDataAsync($"{_uri}/{id}"); //mysql has no guid or uuid type, so we use id without .ToString()

            return int.Parse(deleteResult);
        }

        #endregion

        #region Filter

        public async Task<List<T>> FilterAsync(List<Filter> filters)
        {
            var filterUriStringBuilder = new StringBuilder();
            filterUriStringBuilder.Append(_uri);

            for (var i = 0; i < filters.Count; i++)
            {
                var filter = filters[i];

                filterUriStringBuilder.Append(i == 0 ? "?" : "&");
                filterUriStringBuilder.Append(filter);
            }

            var uri = filterUriStringBuilder.ToString();

            var table = await _phpCrudApiService.GetDataAsync<TTbl>(uri);

            return table.Rows;
        }

        #endregion

        #region Properties

        public async Task<TablePropertiesBase> GetTablePropertiesAsync()
        {
            var tbl = new TTbl();
            var t = new T();

            var json = await _phpCrudApiService.GetTablePropertiesAsync(tbl.Uri, t.PrimaryKeyName);

            JContainer obj = (JContainer)Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            var tableProperties = new TablePropertiesBase
            {
                LastPrimaryKey = obj.First.First.First.First.First.ToString(),
                RowCount = int.Parse(obj.Last.First.ToString())
            };

            return tableProperties;
        }

        #endregion
    }
}
