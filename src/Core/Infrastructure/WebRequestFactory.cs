using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    internal class WebRequestFactory : IWebRequestFactory {
        public WebRequest Create(string uri) {
            return WebRequest.Create(uri);
        }
    }
}
