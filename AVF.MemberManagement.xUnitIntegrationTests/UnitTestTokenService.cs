using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Factories;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Services;

namespace AVF.MemberManagement.xUnitIntegrationTests
{
    public class UnitTestTokenService : TokenService
    {
        public override async Task<string> GetTokenAsync(RestApiAccount restApiAccount)
        {
            string token;

            Uri u = new Uri(restApiAccount.ApiUrl);

            var tenv =  Environment.GetEnvironmentVariable("AvfToken");
           var senv = Environment.GetEnvironmentVariable("AvfPHPSESSID");

            Debug.WriteLine(tenv);

            if (tenv != null)
            {
                token = tenv;

                HttpWebRequestWithCookieContainer.CookieContainer.Add(new Cookie("PHPSESSID", senv, "/", u.Host));

                Uri host = new Uri(u.Scheme + "://" + u.Host);

                foreach (Cookie cookie in HttpWebRequestWithCookieContainer.CookieContainer.GetCookies(host))
                {
                    Debug.WriteLine(cookie.Name);
                    Debug.WriteLine(cookie.Value);
                    Debug.WriteLine(cookie.Path);
                    Debug.WriteLine(cookie.Domain);
                }

                /*PHPSESSID
                The thread 0xc3e0 has exited with code 0 (0x0).
                4o2e39tk4h838bekndbrmfkru7
                /
                The thread 0x1db6c has exited with code 0 (0x0).
                The thread 0x1e3e4 has exited with code 0 (0x0).
                raspi3*/
                // HttpWebRequestWithCookieContainer.CookieContainer.Add(new Cookie());
            }
            else
            {
                token = await base.GetTokenAsync(restApiAccount);
                Uri host = new Uri(u.Scheme + "://" + u.Host);

                foreach (Cookie cookie in HttpWebRequestWithCookieContainer.CookieContainer.GetCookies(host))
                {
                    Debug.WriteLine(cookie.Name);
                    Debug.WriteLine(cookie.Value);
                    Debug.WriteLine(cookie.Path);
                    Debug.WriteLine(cookie.Domain);

                    Environment.SetEnvironmentVariable("AvfPHPSESSID", cookie.Value);
                }
            
                Environment.SetEnvironmentVariable("AvfToken", token);
            }

            return token;
        }
    }
}
