using System.Collections.Specialized;
using System.Net;
using System.Text;
using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    internal class AspClient : IAspClient {
        private IWebClientFactory _factory;

        public ICredentials Credentials { get; set; }

        public AspClient(IWebClientFactory factory) {
            _factory = factory;
        }

        public string PostRequest(string address, NameValueCollection postValues) {
            using (var webClient = _factory.Create()) {
                webClient.Credentials = Credentials;
                webClient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                var responseBytes = webClient.UploadValues(address, postValues);
                return Encoding.Default.GetString(responseBytes);
            }
        }
    }
}
