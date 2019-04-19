using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Repositories
{
    public class CachedSettingsRepository : IRepositoryBase<Setting, string>
    {
        private readonly ILogger _logger;

        private readonly IProxyBase<Setting, string> _settingsProxy;

        private readonly Dictionary<string, Setting> _cache = new Dictionary<string, Setting>();

        private bool _allEntriesAreInCache;


        public CachedSettingsRepository(IProxyBase<Setting, string> settingsProxy, ILogger logger)
        {
            _settingsProxy = settingsProxy;
            _logger = logger;
        }

        public Task<int> CreateAsync(Setting s)
        {
            throw new NotImplementedException(); 
        }

        public async Task<List<Setting>> GetAsync()
        {
            if (_allEntriesAreInCache)
            {
                return _cache.Select(setting => setting.Value).ToList();
            }

            var settings = await _settingsProxy.GetAsync();

            foreach (var setting in settings)
            {
                if (!_cache.ContainsKey(setting.Id))
                {
                    _cache.Add(setting.Id, setting);
                }
            }

            _allEntriesAreInCache = true;

            return settings;
        }

        public async Task<Setting> GetAsync(string id)
        {
            if (_cache.ContainsKey(id))
            {
                return _cache[id];
            }

            try
            {
                var setting = await _settingsProxy.GetAsync(id);
                if (!_cache.ContainsKey(setting.Id))
                {
                    _cache.Add(setting.Id, setting);
                }
                else
                {
                    _logger.LogTrace("Key was already added by another thread: " + setting.Id);
                }
                return setting;
            }
            catch (WebException webException)
            {
                if (((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException(id);
                }

                throw;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if (invalidOperationException.Message == "Sequence contains no matching element")
                {
                    throw new KeyNotFoundException(id);
                }

                throw;
            }
        }

        public Task<List<Setting>> GetAsync(List<Filter> filters)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Setting s)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Setting s)
        {
            throw new NotImplementedException();
        }
    }
}
