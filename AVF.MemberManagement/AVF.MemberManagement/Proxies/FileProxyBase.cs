﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Options;
using Microsoft.Extensions.Options;
using PCLStorage;

namespace AVF.MemberManagement.Proxies
{
    public class FileProxyBase<TTbl, T, TId> : IProxyBase<T, TId> where TTbl : ITable<T>, new()
        where T : IId<TId>
    {
        private readonly IOptions<FileProxyOptions> _fileProxyOptions;

        public FileProxyBase(IOptions<FileProxyOptions> fileProxyOptions)
        {
            _fileProxyOptions = fileProxyOptions;
        }

        public async Task<List<T>> GetAsync()
        {
            Debug.WriteLine(new TTbl().Uri);

            var list = await GetDataFromJsonFileAsync();

            if (_fileProxyOptions.Value.ShouldSimulateDelay)
            {
                var tboName = typeof(T).Name;
                var fileProxyDelayTimes = _fileProxyOptions.Value.FileProxyDelayTimes.ContainsKey(tboName) ? _fileProxyOptions.Value.FileProxyDelayTimes[tboName] : _fileProxyOptions.Value.FallBackTimes;

                await Task.Delay(fileProxyDelayTimes.GetAsyncFullTableDelay); //GetAsyncFullTableDelay as DependencyProperty https://stackoverflow.com/questions/2621488/create-a-delegate-from-a-property-getter-or-setter-method
            }

            return list;
        }

        public async Task<T> GetAsync(TId id)
        {
            var list = await GetDataFromJsonFileAsync();

            await Task.Delay(2000);

            return list.Single(t => t.Id.Equals(id));
        }

        public Task<string> UpdateAsync(T obj, TId id)
        {
            throw new NotImplementedException();
        }

        private static async Task<List<T>> GetDataFromJsonFileAsync()
        {
            var tboTypeName = typeof(T).Name;

            var localStorage = FileSystem.Current.LocalStorage;
            var avfFolder = await localStorage.CreateFolderAsync("AVF",
                CreationCollisionOption.OpenIfExists);

            var fileName = $"List{tboTypeName}.json";
            var jsonFile = await avfFolder.GetFileAsync(fileName);
            var json = await jsonFile.ReadAllTextAsync();

            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
            return list;
        }
    }
}
