using System.Net;

namespace AspUnitRunner.Core {
    internal interface IWebRequestFactory {
        WebRequest Create(string uri);
    }
}
