using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AVF.MemberManagement.StandardLibrary.Factories
{
    public class HttpWebRequestWithCookieContainer
    {
        public static CookieContainer CookieContainer = new CookieContainer();
        
        public static HttpWebRequest Create(string url)
        {
            var httpWebReqeust = (HttpWebRequest)WebRequest.Create(url);
            httpWebReqeust.CookieContainer = CookieContainer;
            return httpWebReqeust;
        }

        public static async Task<string> GetStringAsync(WebRequest httpWebRequest)
        {
            var response = await httpWebRequest.GetResponseAsync();
            var responseStream = response.GetResponseStream();

            var responseBytes = ReadFully(responseStream);
            var responseString = Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length);

            return responseString;
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
