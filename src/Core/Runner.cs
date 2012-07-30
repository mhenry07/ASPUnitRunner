using System.Collections.Specialized;
using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public class Runner {
        private const string BaseQueryString = "?UnitRunner=results";
        private const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";

        private readonly IAspClient _client;

        /// <summary>
        /// Creates a new AspUnitRunner.Runner instance.
        /// </summary>
        /// <returns>A new AspUnitRunner.Runner instance.</returns>
        public static Runner Create() {
            return Infrastructure.Ioc.ResolveRunner();
        }

        internal Runner(IAspClient client) {
            _client = client;
        }

        /// <summary>
        /// Runs ASPUnit tests and returns results.
        /// </summary>
        /// <param name="baseUrl">The URL for the ASPUnit tests.</param>
        /// <param name="testContainer">The name of the test container from which to run tests.</param>
        /// <returns>An AspUnitRunner.Results containing the test results.</returns>
        public Results Run(string baseUrl, string testContainer) {
            return Run(baseUrl, testContainer, null);
        }

        /// <summary>
        /// Runs ASPUnit tests and returns results.
        /// </summary>
        /// <param name="baseUrl">The URL for the ASPUnit tests.</param>
        /// <param name="testContainer">The name of the test container from which to run tests.</param>
        /// <param name="credentials">The network credentials used to authenticate the request.</param>
        /// <returns>An AspUnitRunner.Results containing the test results.</returns>
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
