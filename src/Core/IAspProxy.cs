using System.Net;

namespace AspUnitRunner.Core {
    public interface IAspProxy {
        string GetTestResults(string url, string postData, ICredentials credentials);
    }
}
