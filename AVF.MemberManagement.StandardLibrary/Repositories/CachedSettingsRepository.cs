using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Repositories
{
    public class CachedSettingsRepository : IRepositoryBase<Setting, string>
    {
        private readonly IProxyBase<Setting, string> _settingsProxy;

        private readonly Dictionary<string, Setting> _cache = new Dictionary<string, Setting>();

        private bool _allEntriesAreInCache;


        public CachedSettingsRepository(IProxyBase<Setting, string> settingsProxy)
        {
            _settingsProxy = settingsProxy;
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
                _cache.Add(setting.Id, setting);
                return setting;
            }
            catch (WebException e)
            {
                if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException(id);
                }

                throw;
            }
        }
    }
}
