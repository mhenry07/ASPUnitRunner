using System.Collections.Specialized;
using System.Net;

namespace AspUnitRunner.Core {
    internal interface IAspClient {
        ICredentials Credentials { get; set; }
        string PostRequest(string address, NameValueCollection postValues);
    }
}
