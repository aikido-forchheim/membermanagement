using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IAccountService
    {
        void Init(RestApiAccount restApiAccount);

        void InitWithAccountStore(string appId);

        bool IsRestApiAccountSet { get; set; }

		void StoreRestApiAccount(string apiUrl, string userName, string password);

		RestApiAccount RestApiAccount { get; set; }
	}
}
