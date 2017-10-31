using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tables;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class SettingsProxy : ISettingsProxy
    {
        private readonly ILogger _logger;
        private readonly IPhpCrudApiService _phpCrudApiService;

        private const string Uri = TblSettings.TableName;

        private List<Setting> _settingsCache;

        public SettingsProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
        {
            _logger = logger;
            _phpCrudApiService = phpCrudApiService;
        }

        public async Task<List<Setting>> LoadSettingsCacheAsync()
        {
            return await LoadSettingsCacheAsync(false);
        }

        public async Task<List<Setting>> LoadSettingsCacheAsync(bool forceCacheReload)
        {
            if (_settingsCache != null && !forceCacheReload) return _settingsCache;

            _settingsCache = (await _phpCrudApiService.GetDataAsync<TblSettings>(Uri)).Settings;

            _logger.LogInformation(_settingsCache.Count + " Settings loaded");

            return _settingsCache;
        }

        public async Task<Setting> GetSettingAsync(string key, string defaultValue)
        {
            var setting = await GetSettingAsync(key);

            if (setting == null)
                return new Setting
                {
                    Key = key,
                    Des = key,
                    Value = defaultValue
                };

            return setting;
        }

        public async Task<Setting> GetSettingAsync(string key)
        {
            if (_settingsCache == null) await LoadSettingsCacheAsync();

            var setting = (from s in _settingsCache where s.Key == key select s).SingleOrDefault();

            return setting;
        }
    }
}
