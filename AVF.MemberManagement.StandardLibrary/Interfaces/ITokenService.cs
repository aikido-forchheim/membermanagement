using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(RestApiAccount restApiAccount);
    }
}
