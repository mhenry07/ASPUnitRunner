using System.Collections.Specialized;
using System.Net;

namespace AspUnitRunner.Core {
    internal interface IAspClient {
        string PostRequest(string address, NameValueCollection postValues, ICredentials credentials);
    }
}
