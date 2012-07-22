using System.Net;

namespace AspUnitRunner.Core {
    public interface IWebRequestFactory {
        WebRequest Create(string uri);
    }
}
