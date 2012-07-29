using System.Collections.Specialized;
using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    public class Runner {
        private const string BaseQueryString = "?UnitRunner=results";
        private const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";

        private readonly IAspClient _client;

        /// <summary>
        /// Creates a new Runner instance.
        /// </summary>
        /// <returns>A new Runner instance.</returns>
        public static Runner Create() {
            return Infrastructure.Ioc.ResolveRunner();
        }

        internal Runner(IAspClient client) {
            _client = client;
        }

        public Results Run(string baseUrl, string testContainer) {
            return Run(baseUrl, testContainer, null);
        }

        public Results Run(string baseUrl, string testContainer, ICredentials credentials) {
            var htmlResults = _client.PostRequest(FormatUrl(baseUrl), GetPostData(testContainer), credentials);
            return ResultParser.Parse(htmlResults);
        }

        private string FormatUrl(string baseUrl) {
            return baseUrl + BaseQueryString;
        }

        private NameValueCollection GetPostData(string testContainer) {
            return new NameValueCollection() {
                { "cboTestContainers", testContainer },
                { "cboTestCases", AllTestCases },
                { "cmdRun", RunCommand }
            };
        }
    }
}
