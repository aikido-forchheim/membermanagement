using AVF.MemberManagement.StandardLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ISettingsProxy
	{
		Task<Setting> GetSettingAsync(string key);

		Task<List<Setting>> GetSettingsAsync();
	}
}
