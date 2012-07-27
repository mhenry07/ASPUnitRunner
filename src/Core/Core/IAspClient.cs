using System.Collections.Generic;
using System.Net;

namespace AspUnitRunner.Core {
    internal interface IAspClient {
        string GetTestResults(string url, IEnumerable<KeyValuePair<string, string>> postData, ICredentials credentials);
    }
}
