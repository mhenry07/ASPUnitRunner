using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    internal class AspClient : IAspClient {
        private IWebClientFactory _factory;

        public AspClient(IWebClientFactory factory) {
            _factory = factory;
        }

        public string GetTestResults(string url, IEnumerable<KeyValuePair<string, string>> postData, ICredentials credentials) {
            using (var webClient = _factory.Create()) {
                webClient.Credentials = credentials;
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                return webClient.UploadString(url, FormatPostData(postData));
            }
        }

        private string FormatPostData(IEnumerable<KeyValuePair<string, string>> postValues) {
            var sb = new StringBuilder();
            foreach (var value in postValues) {
                if (sb.Length > 0)
                    sb.Append("&");
                sb.Append(FormatPostValue(value));
            };
            return sb.ToString();
        }

        private string FormatPostValue(KeyValuePair<string, string> value) {
            return string.Format("{0}={1}",
                HttpUtility.UrlEncode(value.Key),
                HttpUtility.UrlEncode(value.Value));
        }
    }
}
