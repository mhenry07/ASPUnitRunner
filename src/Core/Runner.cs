using System.Collections.Generic;
using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    public class Runner {
        private const string BaseQueryString = "?UnitRunner=results";
        private const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";

        private readonly IAspProxy _proxy;
        private readonly string _baseUrl;
        private readonly ICredentials _credentials;

        public Runner(string baseUrl)
            : this(baseUrl, Infrastructure.Ioc.ResolveAspProxy()) {
        }

        public Runner(string baseUrl, ICredentials credentials)
            : this(baseUrl, credentials, Infrastructure.Ioc.ResolveAspProxy()) {
        }

        internal Runner(string baseUrl, IAspProxy proxy)
            : this(baseUrl, null, proxy) {
        }

        internal Runner(string baseUrl, ICredentials credentials, IAspProxy proxy) {
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

        private IEnumerable<KeyValuePair<string, string>> GetPostData(string testContainer) {
            return new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("cboTestContainers", testContainer),
                new KeyValuePair<string, string>("cboTestCases", AllTestCases),
                new KeyValuePair<string, string>("cmdRun", RunCommand)
            };
        }
    }
}
