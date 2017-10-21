using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Factories;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary2.Services
{
    public class TokenService : ITokenService
    {
        public async Task<string> GetTokenAsync(RestApiAccount restApiAccount)
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

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            //byte[] data = Encoding.ASCII.GetBytes(postData);

            //httpWebReqeust.ContentType = "application/x-www-form-urlencoded";
            httpWebReqeust.ContentLength = data.Length;

            Stream requestStream = httpWebReqeust.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)httpWebReqeust.GetResponse();

            Stream responseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            string pageContent = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();

            myHttpWebResponse.Close();

            return pageContent;

            //using (var stream = await Task.Factory.FromAsync(httpWebReqeust.BeginGetRequestStream, httpWebReqeust.EndGetRequestStream, null))
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            //return await HttpWebRequestWithCookieContainer.GetStringAsync(httpWebReqeust);
        }
    }
}
