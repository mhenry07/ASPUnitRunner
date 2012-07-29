using System.Collections.Specialized;
using System.Net;
using System.Text;
using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    internal class AspClient : IAspClient {
        private IWebClientFactory _factory;

        public AspClient(IWebClientFactory factory) {
            _factory = factory;
        }

        public string GetTestResults(string url, NameValueCollection postValues, ICredentials credentials) {
            using (var webClient = _factory.Create()) {
                webClient.Credentials = credentials;
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                var responseBytes = webClient.UploadValues(url, postValues);
                return Encoding.Default.GetString(responseBytes);
            }
        }
    }
}
