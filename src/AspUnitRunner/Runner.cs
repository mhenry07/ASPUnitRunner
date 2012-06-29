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

        private IAspProxy _proxy;
        private string _baseUri;
        private ICredentials _credentials;

        public Runner(string baseUri)
            : this(baseUri, new AspProxy()) {
        }

        public Runner(string baseUri, ICredentials credentials)
            : this(baseUri, credentials, new AspProxy()) {
        }

        public Runner(string baseUri, IAspProxy proxy)
            : this(baseUri, null, proxy) {
        }

        public Runner(string baseUri, ICredentials credentials, IAspProxy proxy) {
            _baseUri = baseUri;
            _credentials = credentials;
            _proxy = proxy;
        }

        public Results Run(string testContainer) {
            string htmlResults = _proxy.GetTestResults(GetUri(), GetPostData(testContainer), _credentials);
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
