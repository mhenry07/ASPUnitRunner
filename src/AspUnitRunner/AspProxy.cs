using System.IO;
using System.Net;
using System.Text;

namespace AspUnitRunner {
    internal class AspProxy : IAspProxy {
        private readonly IWebRequestFactory _webRequestFactory;

        public AspProxy()
            : this(new WebRequestFactory()) {
        }

        public AspProxy(IWebRequestFactory webRequestFactory) {
            _webRequestFactory = webRequestFactory;
        }

        public string GetTestResults(string url, string postData, ICredentials credentials) {
            var request = _webRequestFactory.Create(url);
            request.Method = WebRequestMethods.Http.Post;
            request.Credentials = credentials;
            request.ContentType = "application/x-www-form-urlencoded";
            SetPostData(request, postData);

            return GetResponse(request);
        }

        private void SetPostData(WebRequest request, string postData) {
            var encoding = new ASCIIEncoding();
            var postBytes = encoding.GetBytes(postData);
            request.ContentLength = postBytes.Length;
            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }
        }

        private string GetResponse(WebRequest request) {
            using (var response = request.GetResponse())
            using (var responseStream = new StreamReader(response.GetResponseStream())) {
                return responseStream.ReadToEnd();
            }
        }
    }
}
