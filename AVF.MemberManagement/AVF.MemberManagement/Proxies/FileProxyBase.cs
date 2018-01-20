using System;
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

        private readonly IFileProxyDelayTimes _fileProxyDelayTimes;

        public FileProxyBase(IOptions<FileProxyOptions> fileProxyOptions)
        {
            _fileProxyOptions = fileProxyOptions;
            _fileProxyDelayTimes = GetFileProxyDelayTimes();
        }

        public virtual async Task<int> CreateAsync(T obj)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAsync()
        {
            Debug.WriteLine(new TTbl().Uri);

            var list = await GetDataFromJsonFileAsync();

            await SimulateDelay(_fileProxyDelayTimes.GetFullTableDelayGetAsync);

            return list;
        }

        public async Task<T> GetAsync(TId id)
        {
            var list = await GetDataFromJsonFileAsync();

            await SimulateDelay(_fileProxyDelayTimes.GetSingleEntryDelayGetAsync);

            return list.Single(t => t.Id.Equals(id));
        }

        public Task<int> UpdateAsync(T obj)
        {
            throw new NotImplementedException();
        }

        public async Task<TablePropertiesBase> GetTablePropertiesAsync()
        {
            var list = await GetDataFromJsonFileAsync();

            var tablePropertiesBase = new TablePropertiesBase
            {
                LastPrimaryKey = list.Select(obj => obj.Id.ToString()).Max(),
                RowCount = list.Count
            };

            return tablePropertiesBase;
        }

        #region private

        protected static async Task<List<T>> GetDataFromJsonFileAsync()
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

        protected static async Task SaveDataToJsonFileAsync(List<T> list)
        {
            var tboTypeName = typeof(T).Name;
            var localStorage = FileSystem.Current.LocalStorage;
            var avfFolder = await localStorage.CreateFolderAsync("AVF",
                CreationCollisionOption.OpenIfExists);
            var fileName = $"List{tboTypeName}.json";

            var jsonFile = await avfFolder.GetFileAsync(fileName);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(list);

            await jsonFile.WriteAllTextAsync(json);
        }

        private IFileProxyDelayTimes GetFileProxyDelayTimes()
        {
            var tboName = typeof(T).Name;
            var fileProxyDelayTimes = _fileProxyOptions.Value.FileProxyDelayTimes.ContainsKey(tboName) ? _fileProxyOptions.Value.FileProxyDelayTimes[tboName] : _fileProxyOptions.Value.FallBackTimes;
            return fileProxyDelayTimes;
        }

        private async Task SimulateDelay(Func<int> delayFunc)
        {
            if (_fileProxyOptions.Value.ShouldSimulateDelay)
            {
                await Task.Delay(delayFunc.Invoke());
            }
        }

        #endregion
    }
}
