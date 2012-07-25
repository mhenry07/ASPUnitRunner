using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace AspUnitRunner {
    internal class AspProxy : IAspProxy {
        private readonly IWebRequestFactory _webRequestFactory;

        public AspProxy()
            : this(new WebRequestFactory()) {
        }

        public AspProxy(IWebRequestFactory webRequestFactory) {
            _webRequestFactory = webRequestFactory;
        }

        public string GetTestResults(string url, IEnumerable<KeyValuePair<string, string>> postValues, ICredentials credentials) {
            var request = CreatePostRequest(url);
            request.Credentials = credentials;
            SetPostData(request, postValues);

            return GetResponse(request);
        }

        private WebRequest CreatePostRequest(string url) {
            var request = _webRequestFactory.Create(url);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded";
            return request;
        }

        private void SetPostData(WebRequest request, IEnumerable<KeyValuePair<string, string>> postValues) {
            var postData = FormatPostData(postValues);
            var postBytes = Encoding.ASCII.GetBytes(postData);
            request.ContentLength = postBytes.Length;
            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(postBytes, 0, postBytes.Length);
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

        private string GetResponse(WebRequest request) {
            using (var response = request.GetResponse())
            using (var responseStream = new StreamReader(response.GetResponseStream())) {
                return responseStream.ReadToEnd();
            }
        }
    }
}
