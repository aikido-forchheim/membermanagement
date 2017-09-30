using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.GTK.Services
{
    public class AccountService : IAccountService
    {
        private RestApiAccount _restApiAccount;

        public void Init(RestApiAccount restApiAccount)
        {
            _restApiAccount = restApiAccount;
        }

        public void InitWithAccountStore(string appId)
        {
            _restApiAccount = new RestApiAccount
            {
                Username = Properties.Settings.Default.Username,
                Password = Properties.Settings.Default.Password,
                ApiUrl = Properties.Settings.Default.ApiUrl
            };
        }

        public bool IsRestApiAccountSet
        {
            get
            {
                try
                {
                    return !string.IsNullOrEmpty(Properties.Settings.Default.Username)
                           && !string.IsNullOrEmpty(Properties.Settings.Default.Password)
                           && !string.IsNullOrEmpty(Properties.Settings.Default.ApiUrl);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            set
            {
            }
        }
        public void StoreRestApiAccount(string apiUrl, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) return;

            Properties.Settings.Default.Username = userName;
            Properties.Settings.Default.Password = password;
            Properties.Settings.Default.ApiUrl = apiUrl;

            Properties.Settings.Default.Save();
        }

        public RestApiAccount RestApiAccount
        {
            get
            {
                return _restApiAccount;
            }
            set
            {
            }
        }
    }
}
