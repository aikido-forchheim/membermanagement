using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ISettingsProxy
	{
		Task<Setting> GetSettingAsync(string key);

	    Task<Setting> GetSettingAsync(string key, string defaultValue);

        Task<List<Setting>> LoadSettingsCacheAsync();

	    Task<List<Setting>> LoadSettingsCacheAsync(bool forceCacheReload);
	}
}
