using System.Net;

namespace AspUnitRunner {
    public interface IWebRequestFactory {
        WebRequest Create(string uri);
    }
}
