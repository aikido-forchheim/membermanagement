﻿using System;
using System.Linq;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Logging;
using Xamarin.Auth;

namespace AVF.CourseParticipation.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger _logger;
        private RestApiAccount _restApiAccount;

        public AccountService(ILogger logger)
        {
            _logger = logger;
        }

        public void Init(RestApiAccount restApiAccount)
        {
            _restApiAccount = restApiAccount;
        }

        public void InitWithAccountStore(string appId)
        {
            var account = AccountStore.Create().FindAccountsForService(appId)?.FirstOrDefault();

            _restApiAccount = new RestApiAccount
            {
                ApiUrl = account?.Properties["ApiUrl"],
                Password = account?.Properties["Password"],
                Username = account?.Username
            };
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

        public bool IsRestApiAccountSet
        {
            get
            {
                try
                {
                    var account = AccountStore.Create().FindAccountsForService(App.AppId)?.FirstOrDefault();
                    return account != null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
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

            var account = new Account
            {
                Username = userName
            };
            account.Properties.Add("Password", password);
            account.Properties.Add("ApiUrl", apiUrl);

            var accounts = AccountStore.Create().FindAccountsForService(App.AppId).ToList();
            foreach (var a in accounts)
            {
                AccountStore.Create().Delete(a, App.AppId);
            }

            AccountStore.Create().Save(account, App.AppId);
        }
    }
}

