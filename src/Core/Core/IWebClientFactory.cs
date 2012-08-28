using System.Net;

namespace AspUnitRunner.Core {
    internal interface IWebClientFactory {
        WebClient Create();
    }
}
