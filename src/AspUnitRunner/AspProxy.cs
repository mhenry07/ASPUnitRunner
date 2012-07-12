using System.IO;
using System.Net;
using System.Text;

namespace AspUnitRunner {
    internal class AspProxy : IAspProxy {
        private IWebRequestFactory _webRequestFactory;

        public AspProxy() : this(new WebRequestFactory()) {
        }

        public AspProxy(IWebRequestFactory webRequestFactory) {
            _webRequestFactory = webRequestFactory;
        }

        public string GetTestResults(string uri, string postData, ICredentials credentials) {
            var request = _webRequestFactory.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.Credentials = credentials;
            request.ContentType = "application/x-www-form-urlencoded";
            SetPostData(request, postData);

            return GetResponse(request);
        }

        private void SetPostData(WebRequest request, string postData) {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postBytes = encoding.GetBytes(postData);
            request.ContentLength = postBytes.Length;
            using (Stream requestStream = request.GetRequestStream()) {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }
        }

        private string GetResponse(WebRequest request) {
            using (WebResponse response = request.GetResponse()) {
                using (StreamReader responseStream = new StreamReader(response.GetResponseStream())) {
                    return responseStream.ReadToEnd();
                }
            }
        }
    }
}
