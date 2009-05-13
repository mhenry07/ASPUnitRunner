using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AspUnitRunner.Tests {
    class FakeProxy : IProxy {
        public string HtmlResults { get; set; }
        public string Uri { get; private set; }
        public string PostData { get; private set; }
        public ICredentials Credentials { get; private set; }

        #region IProxy Members

        public string GetTestResults(string uri, string postData) {
            Uri = uri;
            PostData = postData;
            return HtmlResults;
        }

        #endregion
    }
}
