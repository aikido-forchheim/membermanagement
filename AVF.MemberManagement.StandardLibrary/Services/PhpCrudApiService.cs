using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Factories;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AVF.MemberManagement.StandardLibrary.Services
{
    public class PhpCrudApiService : IPhpCrudApiService
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;

        private string _token;

        public PhpCrudApiService(ILogger logger, IAccountService accountService, ITokenService tokenService)
        {
            _logger = logger;
            _accountService = accountService;
            _tokenService = tokenService;
        }


        #region Create

        public async Task<string> SendDataAsync<T>(string url, T dataObject)
        {
            var jsonData = JsonConvert.SerializeObject(dataObject);
            var sendResult = await SendDataAsync(url, jsonData, "POST");
            if (sendResult == null) throw new IOException("PhpCrudApiService could not create object");
            return sendResult;
        }

        private async Task<string> SendDataAsync(string uri, string jsonData, string method = "POST")
        {
            var request = await GetRequest(uri, method);

            var stream = await request.GetRequestStreamAsync();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(jsonData);
                writer.Flush();
                writer.Dispose();
            }

            return await GetResponse(request);
        }

        #endregion

        #region Read

        public async Task<T> GetDataAsync<T>(string uri)
        {
            var tableResult = await GetDataAsync(uri, true);
            var wrapper = JsonConvert.DeserializeObject<T>(tableResult);

            return wrapper;
        }

        public async Task<string> GetDataAsync(string uri, bool serverTransform = false)
        {
            try
            {
                var fullUri = await GetFullUriWithCsrfToken(uri);

                if (serverTransform) fullUri = AddQueryOption(fullUri, "transform", "1");

                var httpWebRequest = HttpWebRequestWithCookieContainer.Create(fullUri);
                httpWebRequest.Method = "GET";

                return await HttpWebRequestWithCookieContainer.GetStringAsync(httpWebRequest);
            }
            catch (Exception e)
            {
                //await Task.Delay(250);

                Debug.WriteLine("Forcetokenrefresh");
                var fullUri = await GetFullUriWithCsrfToken(uri, true);
                
                //await Task.Delay(250);

                if (serverTransform) fullUri = AddQueryOption(fullUri, "transform", "1");

                var httpWebRequest = HttpWebRequestWithCookieContainer.Create(fullUri);
                httpWebRequest.Method = "GET";

                return await HttpWebRequestWithCookieContainer.GetStringAsync(httpWebRequest);
            }
        }

        public List<T> GetList<T>(string tableResultJson)
        {
            var result = JObject.Parse(tableResultJson);

            var recordsArray = result.First.First.SelectToken("records").ToList();
            var columnsArray = result.First.First.SelectToken("columns").ToList();

            var list = new List<T>();
            foreach (var columnValuesArray in recordsArray)
            {
                var jObject = new JObject();

                for (var i = 0; i < columnsArray.Count; i++)
                {
                    var columnName = columnsArray[i].Value<string>();
                    var jProperty = new JProperty(columnName, columnValuesArray[i]);
                    jObject.Add(jProperty);
                }

                var reorganizedJson = jObject.ToString();

                var deserializedObject = JsonConvert.DeserializeObject<T>(reorganizedJson);
                list.Add(deserializedObject);
            }

            return list;
        }

        #endregion

        #region Update

        public async Task<string> UpdateDataAsync<T>(string url, T dataObject)
        {
            var jsonData = JsonConvert.SerializeObject(dataObject);
            var responseResult = await SendDataAsync(url, jsonData, "PUT");
            if (responseResult == null) throw new IOException("PhpCrudApiService could not update object");
            return responseResult;
        }

        #endregion

        #region Delete

        public async Task<string> DeleteDataAsync(string url)
        {
            try
            {
                var response = await DeleteDataAsyncInternal(url);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }

        private async Task<string> DeleteDataAsyncInternal(string uri)
        {
            var request = await GetRequest(uri, "DELETE");

            return await GetResponse(request);
        }

        #endregion


        #region Helper

        private async Task<string> GetFullUriWithCsrfToken(string resourcePathAndQueryOptions, bool forceTokenRefresh = false)
        {
            if (_token == null || _accountService.RestApiAccount.HasChanged || forceTokenRefresh) _token = await _tokenService.GetTokenAsync(_accountService.RestApiAccount);

            if (!_accountService.RestApiAccount.ApiUrl.EndsWith("/") && !resourcePathAndQueryOptions.StartsWith("/")) resourcePathAndQueryOptions = "/" + resourcePathAndQueryOptions;

            var fullUri = _accountService.RestApiAccount.ApiUrl + resourcePathAndQueryOptions;

            fullUri = AddCsrfToken(fullUri);

            return fullUri;
        }

        private string AddCsrfToken(string serviceRootUrlWithResourcePathAndQueryOptions)
        {
            serviceRootUrlWithResourcePathAndQueryOptions = AddQueryOption(serviceRootUrlWithResourcePathAndQueryOptions, "csrf", _token);
            return serviceRootUrlWithResourcePathAndQueryOptions;
        }

        private static string AddQueryOption(string uri, string paramName, string paramValue)
        {
            //UriBuilder uriBuilder = new UriBuilder();
            //uriBuilder.

            if (!uri.Contains("?")) uri = uri + $"?{paramName}={paramValue}";
            else uri = uri + $"&{paramName}={paramValue}";
            return uri;
        }

        private static async Task<string> GetResponse(System.Net.HttpWebRequest request)
        {
            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();

            using (var streamReader = new StreamReader(responseStream))
            {
                var responseString = streamReader.ReadToEnd();

                return responseString == "null" ? null : responseString;
            }
        }

        private async Task<System.Net.HttpWebRequest> GetRequest(string uri, string method)
        {
            var fullUri = await GetFullUriWithCsrfToken(uri);

            var request = HttpWebRequestWithCookieContainer.Create(fullUri);
            request.ContentType = "application/json";
            request.Method = method;
            return request;
        }

        #endregion
    }
}

