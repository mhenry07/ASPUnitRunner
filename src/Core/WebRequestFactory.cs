using System.Net;

namespace AspUnitRunner {
    internal class WebRequestFactory : IWebRequestFactory {
        public WebRequest Create(string uri) {
            return WebRequest.Create(uri);
        }
    }
}
