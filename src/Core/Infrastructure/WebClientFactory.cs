using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    internal class WebClientFactory : IWebClientFactory {
        public WebClient Create() {
            return new WebClient();
        }
    }
}
