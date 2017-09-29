using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Factories;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Services
{
    public class TokenService
    {
        public static async Task<string> GetTokenAsync(RestApiAccount restApiAccount)
        {
            var tokenAsync = await GetTokenAsync(restApiAccount.ApiUrl, restApiAccount.Username, restApiAccount.Password);
            restApiAccount.HasChanged = false;
            return tokenAsync;
        }

        private static async Task<string> GetTokenAsync(string apiUrl, string userName, string password)
        {
            var httpWebReqeust = HttpWebRequestWithCookieContainer.Create(apiUrl);

            httpWebReqeust.Method = "POST";

            var postData = $"username={userName}&password={password}";

            var data = Encoding.UTF8.GetBytes(postData);

            httpWebReqeust.ContentType = "application/x-www-form-urlencoded";

            using (var stream = await Task.Factory.FromAsync(httpWebReqeust.BeginGetRequestStream, httpWebReqeust.EndGetRequestStream, null))
            {
                stream.Write(data, 0, data.Length);
            }

            return await HttpWebRequestWithCookieContainer.GetStringAsync(httpWebReqeust);
        }
    }
}
