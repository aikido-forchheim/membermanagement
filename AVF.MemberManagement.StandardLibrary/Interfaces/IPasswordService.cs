using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IPasswordService
    {
		Task<bool> IsValidAsync(string enteredPassword, string storedPasswordHash, string pepper);

		Task<string> HashPasswordAsync(string password, string pepper, byte[] saltBytes = null);
	}
}
