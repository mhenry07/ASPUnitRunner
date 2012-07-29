using System.Collections.Specialized;
using System.Net;

namespace AspUnitRunner.Core {
    internal interface IAspClient {
        string PostRequest(string url, NameValueCollection postValues, ICredentials credentials);
    }
}
