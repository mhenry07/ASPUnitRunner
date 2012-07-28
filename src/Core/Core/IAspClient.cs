using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace AspUnitRunner.Core {
    internal interface IAspClient {
        string GetTestResults(string url, IEnumerable<KeyValuePair<string, string>> postData, ICredentials credentials);
        string GetTestResults(string url, NameValueCollection postValues, ICredentials credentials);
    }
}
