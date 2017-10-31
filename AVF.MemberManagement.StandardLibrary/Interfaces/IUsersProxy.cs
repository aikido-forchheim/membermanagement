using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IUsersProxy
	{
		Task<List<User>> GetUsersAsync();

		Task<User> GetUserAsync(string username);

		Task AddNewUserAsync(string newUserName);

		Task UpdateUserAsync(User user);
	}
}
