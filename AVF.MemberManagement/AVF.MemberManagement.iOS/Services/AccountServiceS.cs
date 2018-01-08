using System;
using System.Text;
using System.IO;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Newtonsoft.Json;

namespace AVF.MemberManagement.iOS.Services
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
            if (File.Exists(GetFullPath(RestApiAccountFileName)))
            {
                var json = File.ReadAllText(GetFullPath(RestApiAccountFileName), Encoding.UTF8);

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
                    return File.Exists(GetFullPath(RestApiAccountFileName));
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

            File.WriteAllText(GetFullPath(RestApiAccountFileName), json, Encoding.UTF8);
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

        private string GetFullPath(string filename)
        {
            var personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var fullPath = Path.Combine(personalFolder, filename);
            return fullPath;
        }
	}
}
