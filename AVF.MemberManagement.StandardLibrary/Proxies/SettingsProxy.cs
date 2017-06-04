using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class SettingsProxy : ISettingsProxy
    {
        private readonly ILogger _logger;
        private readonly IPhpCrudApiService _phpCrudApiService;

        private List<Setting> _settings = null;

        public SettingsProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
        {
            _logger = logger;
            _phpCrudApiService = phpCrudApiService;
        }

        public async Task<Setting> GetSettingAsync(string key)
        {
            if (_settings == null) await GetSettingsAsync();

            var setting = (from s in _settings where s.Key == key select s).SingleOrDefault();

            return setting;
        }

        public async Task<List<Setting>> GetSettingsAsync()
        {
            if (_settings != null) return _settings;

            var uri = $"Settings";

            _settings = (await _phpCrudApiService.GetDataAsync<SettingsWrapper>(uri)).Settings;

            _logger.LogInformation(_settings.Count + " Settings loaded");

            return _settings;
        }
    }
}
