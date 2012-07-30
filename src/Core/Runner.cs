using System.Collections.Specialized;
using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public class Runner {
        private const string ResultsQueryString = "?UnitRunner=results";
        private const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";

        private readonly IAspClient _client;

        /// <summary>
        /// Sets the network credentials used to authenticate the request. (Optional)
        /// </summary>
        public ICredentials Credentials { private get; set; }

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
        /// <param name="address">The URL for the ASPUnit tests.</param>
        /// <param name="testContainer">The name of the test container from which to run tests.</param>
        /// <returns>An AspUnitRunner.Results containing the test results.</returns>
        public Results Run(string address, string testContainer) {
            _client.Credentials = Credentials;
            var htmlResults = _client.PostRequest(FormatUrl(address), GetPostData(testContainer));
            return ResultParser.Parse(htmlResults);
        }

        private string FormatUrl(string address) {
            return address + ResultsQueryString;
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
