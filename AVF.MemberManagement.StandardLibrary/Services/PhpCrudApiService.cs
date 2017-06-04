using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        private readonly CookieContainer _cookieContainer = new CookieContainer();

        private string _token;

        public PhpCrudApiService(ILogger logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public async Task<string> UpdateDataAsync<T>(string url, T dataObject)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(dataObject);
                var response = await SendDataAsync(url, jsonData, "PUT");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
        
        public async Task<string> SendDataAsync<T>(string url, T dataObject)
        {
            var jsonData = JsonConvert.SerializeObject(dataObject);
            return await SendDataAsync(url, jsonData, "POST");
        }

        private async Task<string> SendDataAsync(string uri, string jsonData, string method = "POST")
        {
            var fullUri = await GetFullUriWithCsrfToken(uri);

            var request = HttpWebRequestWithCookieContainer(fullUri);
            request.ContentType = "application/json";
            request.Method = method;

            var stream = await request.GetRequestStreamAsync();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(jsonData);
                writer.Flush();
                writer.Dispose();
            }

            var response = await request.GetResponseAsync();
            var responseStream = response.GetResponseStream();

            using (var streamReader = new StreamReader(responseStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        public async Task<T> GetDataAsync<T>(string uri)
        {
            var tableResult = await GetDataAsync(uri, true);
            var wrapper = JsonConvert.DeserializeObject<T>(tableResult);

            return wrapper;
        }

        public async Task<string> GetDataAsync(string uri, bool serverTransform = false)
        {
            var fullUri = await GetFullUriWithCsrfToken(uri);

            if (serverTransform) fullUri = AddParam(fullUri, "transform", "1");

            var httpWebRequest = HttpWebRequestWithCookieContainer(fullUri);
            httpWebRequest.Method = "GET";

            return await GetStringAsync(httpWebRequest);
        }

        private async Task<string> GetFullUriWithCsrfToken(string uri)
        {
            if (_token == null) _token = await GetTokenAsync();

            if (!uri.StartsWith("/")) uri = "/" + uri;

            var fullUri = _accountService.RestApiAccount.ApiUrl + uri;
            fullUri = AddCsrfToken(fullUri);
            return fullUri;
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

        private string AddCsrfToken(string fullUri)
        {
            const string paramName = "csrf";

            var paramValue = _token;
            fullUri = AddParam(fullUri, paramName, paramValue);
            return fullUri;
        }

        private static string AddParam(string uri, string paramName, string paramValue)
        {
            if (!uri.Contains("?")) uri = uri + $"?{paramName}={paramValue}";
            else uri = uri + $"&{paramName}={paramValue}";
            return uri;
        }

        private async Task<string> GetTokenAsync()
        {
            return await GetTokenAsync(_accountService.RestApiAccount.ApiUrl, _accountService.RestApiAccount.Username, _accountService.RestApiAccount.Password);
        }

        private async Task<string> GetTokenAsync(string apiUrl, string userName, string password)
        {
            try
            {
                var httpWebReqeust = HttpWebRequestWithCookieContainer(apiUrl);

                httpWebReqeust.Method = "POST";

                var postData = $"username={userName}&password={password}";

                var data = Encoding.UTF8.GetBytes(postData);

                httpWebReqeust.ContentType = "application/x-www-form-urlencoded";

                using (var stream = await Task.Factory.FromAsync<Stream>(httpWebReqeust.BeginGetRequestStream, httpWebReqeust.EndGetRequestStream, null))
                {
                    stream.Write(data, 0, data.Length);
                }

                return await GetStringAsync(httpWebReqeust);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        private HttpWebRequest HttpWebRequestWithCookieContainer(string url)
        {
            var httpWebReqeust = (HttpWebRequest)WebRequest.Create(url);
            httpWebReqeust.CookieContainer = _cookieContainer;
            return httpWebReqeust;
        }

        private async Task<string> GetStringAsync(WebRequest httpWebRequest)
        {
            try
            {
                var response = await httpWebRequest.GetResponseAsync();
                var responseStream = response.GetResponseStream();

                var responseBytes = ReadFully(responseStream);
                var responseString = Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length);

                return responseString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }

        private static byte[] ReadFully(Stream stream)
        {
            var buffer = new byte[32768];
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    var read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }
    }
}

