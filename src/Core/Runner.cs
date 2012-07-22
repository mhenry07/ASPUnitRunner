using System.Net;
using System.Web;

namespace AspUnitRunner.Core {
    public class Runner {
        private const string BaseQueryString = "?UnitRunner=results";
        private const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";

        private readonly IAspProxy _proxy;
        private readonly string _baseUrl;
        private readonly ICredentials _credentials;

        public Runner(string baseUrl)
            : this(baseUrl, new AspProxy()) {
        }

        public Runner(string baseUrl, ICredentials credentials)
            : this(baseUrl, credentials, new AspProxy()) {
        }

        public Runner(string baseUrl, IAspProxy proxy)
            : this(baseUrl, null, proxy) {
        }

        public Runner(string baseUrl, ICredentials credentials, IAspProxy proxy) {
            _baseUrl = baseUrl;
            _credentials = credentials;
            _proxy = proxy;
        }

        public Results Run(string testContainer) {
            var htmlResults = _proxy.GetTestResults(GetUrl(), GetPostData(testContainer), _credentials);
            return ResultParser.Parse(htmlResults);
        }

        private string GetUrl() {
            return _baseUrl + BaseQueryString;
        }

        private string GetPostData(string testContainer) {
            return string.Format("cboTestContainers={0}&cboTestCases={1}&cmdRun={2}",
                HttpUtility.UrlEncode(testContainer), HttpUtility.UrlEncode(AllTestCases), HttpUtility.UrlEncode(RunCommand));
        }
    }
}
