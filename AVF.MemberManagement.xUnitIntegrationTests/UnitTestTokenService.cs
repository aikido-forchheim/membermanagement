using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Factories;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Services;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class UnitTestTokenService : TokenService
    {
        static UnitTestTokenService()
        {
            Environment.SetEnvironmentVariable("AvfStart", Thread.CurrentThread.ManagedThreadId.ToString());
        }

        private const string AvfStart = "AvfStart";

        private const string AvfPhpSessionId = "AvfPhpSessionId";
        private const string AvfToken = "AvfToken";

        public override async Task<string> GetTokenAsync(RestApiAccount restApiAccount)
        {
            var start = Environment.GetEnvironmentVariable(AvfStart);
            var tokenEnvironmentVariable = Environment.GetEnvironmentVariable(AvfToken);

            if (start == null || start != Thread.CurrentThread.ManagedThreadId.ToString())
            {
                while (tokenEnvironmentVariable == null)
                {
                    Thread.Sleep(100);
                    tokenEnvironmentVariable = Environment.GetEnvironmentVariable(AvfToken);
                }
            }

            string tokenToReturn;

            var apiUrl = new Uri(restApiAccount.ApiUrl);

            tokenEnvironmentVariable = Environment.GetEnvironmentVariable(AvfToken);
            var sessionIdEnvironmentVariable = Environment.GetEnvironmentVariable(AvfPhpSessionId);

            Debug.WriteLine(tokenEnvironmentVariable);

            if (tokenEnvironmentVariable == null)
            {
                tokenToReturn = await base.GetTokenAsync(restApiAccount);

                WriteDebugInfoAndSetSessionId(apiUrl);

                Environment.SetEnvironmentVariable(AvfToken, tokenToReturn);
            }
            else
            {
                tokenToReturn = tokenEnvironmentVariable;

                HttpWebRequestWithCookieContainer.CookieContainer.Add(new Cookie("PHPSESSID", sessionIdEnvironmentVariable, "/", apiUrl.Host));

                WriteDebugInfo(apiUrl);
            }

            return tokenToReturn;
        }

        #region Helper

        private static void WriteDebugInfo(Uri u)
        {
            var host = new Uri(u.Scheme + "://" + u.Host);
            foreach (Cookie cookie in HttpWebRequestWithCookieContainer.CookieContainer.GetCookies(host))
            {
                Debug.WriteLine(cookie.Name);
                Debug.WriteLine(cookie.Value);
                Debug.WriteLine(cookie.Path);
                Debug.WriteLine(cookie.Domain);
            }
        }

        private static void WriteDebugInfoAndSetSessionId(Uri u)
        {
            var host = new Uri(u.Scheme + "://" + u.Host);
            foreach (Cookie cookie in HttpWebRequestWithCookieContainer.CookieContainer.GetCookies(host))
            {
                Debug.WriteLine(cookie.Name);
                Debug.WriteLine(cookie.Value);
                Debug.WriteLine(cookie.Path);
                Debug.WriteLine(cookie.Domain);

                Environment.SetEnvironmentVariable(AvfPhpSessionId, cookie.Value);
            }
        }

        #endregion
    }
}
