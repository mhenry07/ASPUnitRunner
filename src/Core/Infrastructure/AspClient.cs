using System.Collections.Specialized;
using System.Net;
using System.Text;
using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    internal class AspClient : IAspClient {
        private readonly IWebClientFactory _factory;
        private readonly IResponseDecoder _responseDecoder;

        public ICredentials Credentials { get; set; }

        public Encoding Encoding { get; set; }

        public AspClient(IWebClientFactory webClientFactory, IResponseDecoder responseDecoder) {
            _factory = webClientFactory;
            _responseDecoder = responseDecoder;
        }

        public string PostRequest(string address, NameValueCollection postValues) {
            using (var webClient = _factory.Create()) {
                webClient.Credentials = Credentials;
                webClient.Encoding = Encoding ?? webClient.Encoding;
                var responseBytes = webClient.UploadValues(address, postValues);

                return _responseDecoder.DecodeResponse(webClient, responseBytes);
            }
        }
    }
}
