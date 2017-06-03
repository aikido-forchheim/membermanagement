using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IAccountService
	{
		bool IsRestApiAccountSet { get; set; }

		void StoreRestApiAccount(string apiUrl, string userName, string password);

		RestApiAccount RestApiAccount { get; set; }
	}
}
