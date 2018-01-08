using System.Collections.Generic;
using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
	public interface IPhpCrudApiService
	{
		Task<string> GetDataAsync(string uri, bool serverTransform = false);

		Task<T> GetDataAsync<T>(string uri);

		List<T> GetList<T>(string tableResultJson);

		Task<string> SendDataAsync<T>(string url, T dataObject);

		Task<string> UpdateDataAsync<T>(string url, T dataObject);

	    Task<string> DeleteDataAsync(string url);
	}
}
