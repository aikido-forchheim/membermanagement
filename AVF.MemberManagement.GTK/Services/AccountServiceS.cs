using System;
using System.Text;
using System.IO;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Newtonsoft.Json;

namespace AVF.MemberManagement.GTK.Services
{
	public class AccountServiceS : IAccountService
	{
        private const string RestApiAccountFileName = "AVF.RestApiAccount.json";

		private RestApiAccount _restApiAccount;

		public void Init(RestApiAccount restApiAccount)
		{
			_restApiAccount = restApiAccount;
		}

		public void InitWithAccountStore(string appId)
		{
            if (File.Exists(RestApiAccountFileName))
            {
                var json = File.ReadAllText(RestApiAccountFileName, Encoding.UTF8);

				_restApiAccount = JsonConvert.DeserializeObject<RestApiAccount>(json);   
            }
            else
            {
                _restApiAccount = new RestApiAccount();
            }
		}

		public bool IsRestApiAccountSet
		{
			get
			{
				try
				{
                    return File.Exists(RestApiAccountFileName);
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

            RestApiAccount restApiAccount = new RestApiAccount
            {
                Username = userName,
                Password = password,
                ApiUrl = apiUrl
            };

            var json = JsonConvert.SerializeObject(restApiAccount);

            File.WriteAllText(RestApiAccountFileName, json, Encoding.UTF8);
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
