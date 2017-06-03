using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IPasswordHashingService
	{
		Task<bool> IsValidAsync(string enteredPassword, string storedPasswordHash);

		Task<string> HashPasswordAsync(string password, byte[] saltBytes = null);
	}
}
