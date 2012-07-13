using System.Net;

namespace AspUnitRunner {
    public interface IAspProxy {
        string GetTestResults(string uri, string postData, ICredentials credentials);
    }
}
