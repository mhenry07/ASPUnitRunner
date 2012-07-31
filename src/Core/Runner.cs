using System;
using System.Collections.Specialized;
using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public class Runner {
        private const string ResultsQueryString = "?UnitRunner=results";
        private const string AllTestContainers = "All Test Containers";
        private const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";

        private readonly IAspClient _client;
        private string _testContainer;
        private string _testCase;

        /// <summary>
        /// Sets the network credentials used to authenticate the request. (Optional)
        /// </summary>
        public ICredentials Credentials { private get; set; }

        /// <summary>
        /// Gets or sets the name of the test container from which to run tests. (Optional)
        /// </summary>
        public string TestContainer {
            get {
                if (string.IsNullOrEmpty(_testContainer))
                    return AllTestContainers;
                return _testContainer;
            }
            set { _testContainer = value; }
        }

        /// <summary>
        /// Gets or sets the name of the test case to execute. (Optional)
        /// Must be used with a specific test container.
        /// </summary>
        public string TestCase {
            get {
                if (string.IsNullOrEmpty(_testCase))
                    return AllTestCases;
                return _testCase;
            }
            set { _testCase = value; }
        }

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
        /// <returns>An AspUnitRunner.Results containing the test results.</returns>
        public Results Run(string address) {
            _client.Credentials = Credentials;
            var htmlResults = _client.PostRequest(FormatUrl(address), GetPostData());
            return ResultParser.Parse(htmlResults);
        }

        private string FormatUrl(string address) {
            return address + ResultsQueryString;
        }

        private NameValueCollection GetPostData() {
            if (TestCase != AllTestCases && TestContainer == AllTestContainers)
                throw new InvalidOperationException("The test container must be specified when running a specific test case.");

            return new NameValueCollection() {
                { "cboTestContainers", TestContainer },
                { "cboTestCases", TestCase },
                { "cmdRun", RunCommand }
            };
        }
    }
}
