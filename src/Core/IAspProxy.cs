using System.Collections.Generic;
using System.Net;

namespace AspUnitRunner {
    public interface IAspProxy {
        string GetTestResults(string url, IEnumerable<KeyValuePair<string, string>> postData, ICredentials credentials);
    }
}
