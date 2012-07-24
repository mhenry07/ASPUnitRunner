using System.Net;

namespace AspUnitRunner {
    public interface IAspProxy {
        string GetTestResults(string url, string postData, ICredentials credentials);
    }
}
