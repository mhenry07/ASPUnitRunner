using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace AspUnitRunner.Core {
    internal interface IAspClient {
        ICredentials Credentials { get; set; }
        Encoding Encoding { get; set; }
        string PostRequest(string address, NameValueCollection postValues);
    }
}
