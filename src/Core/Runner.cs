using System.Collections.Generic;
using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    public class Runner {
        private const string BaseQueryString = "?UnitRunner=results";
        private const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";

        private readonly IAspProxy _proxy;

        public Runner()
            : this(Infrastructure.Ioc.ResolveAspProxy()) {
        }

        internal Runner(IAspProxy proxy) {
            _proxy = proxy;
        }

        public Results Run(string baseUrl, string testContainer) {
            return Run(baseUrl, testContainer, null);
        }

        public Results Run(string baseUrl, string testContainer, ICredentials credentials) {
            var htmlResults = _proxy.GetTestResults(FormatUrl(baseUrl), GetPostData(testContainer), credentials);
            return ResultParser.Parse(htmlResults);
        }

        private string FormatUrl(string baseUrl) {
            return baseUrl + BaseQueryString;
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
