using System.Collections.Specialized;
using System.Net;

namespace AspUnitRunner.Core {
    internal interface IAspClient {
        ICredentials Credentials { set; }
        string PostRequest(string address, NameValueCollection postValues);
    }
}
