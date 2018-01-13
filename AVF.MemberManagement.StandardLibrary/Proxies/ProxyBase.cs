﻿using System;
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

        public async Task<string> UpdateAsync(T obj, TId id)
        {
            //TODO: Change signature: Id first

            return await _phpCrudApiService.UpdateDataAsync($"{_uri}/{id}", obj);
        }

        #endregion

        #region Delete

        public async Task<int> DeleteAsync(TId id)
        {   
            var deleteResult = await _phpCrudApiService.DeleteDataAsync($"{_uri}/{id}"); //mysql has no guid or uuid type, so we use id without .ToString()

            return int.Parse(deleteResult);
        }

        public async Task<int> DeleteAsync(T obj)
        {
            return await DeleteAsync(obj.Id);
        }

        #endregion
    }
}
