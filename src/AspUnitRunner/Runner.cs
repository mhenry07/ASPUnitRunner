using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;

namespace AspUnitRunner {
    public class Runner {
        private const string BaseQueryString = "?UnitRunner=results";
        private const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";

        private IProxy _proxy;
        private string _baseUri;

        public Runner(string baseUri)
            : this(baseUri, new Proxy()) {
        }

        public Runner(string baseUri, ICredentials credentials)
            : this(baseUri, credentials, new Proxy()) {
        }

        public Runner(string baseUri, IProxy proxy)
            : this(baseUri, null, proxy) {
        }

        public Runner(string baseUri, ICredentials credentials, IProxy proxy) {
            _baseUri = baseUri;
            _proxy = proxy;
        }

        public Results Run(string testContainer) {
            string htmlResults = _proxy.GetTestResults(GetUri(), GetPostData(testContainer));
            return new Results(htmlResults);
        }

        private string GetUri() {
            return _baseUri + BaseQueryString;
        }

        private string GetPostData(string testContainer) {
            return String.Format("cboTestContainers={0}&cboTestCases={1}&cmdRun={2}",
                HttpUtility.UrlEncode(testContainer), HttpUtility.UrlEncode(AllTestCases), HttpUtility.UrlEncode(RunCommand));
        }
    }
}
