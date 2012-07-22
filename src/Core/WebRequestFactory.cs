using System.Net;

namespace AspUnitRunner.Core {
    internal class WebRequestFactory : IWebRequestFactory {
        public WebRequest Create(string uri) {
            return WebRequest.Create(uri);
        }
    }
}
