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
        private string _testContainer = AllTestContainers;
        private string _testCase = AllTestCases;

        /// <summary>
        /// Sets the network credentials used to authenticate the request. (Optional)
        /// </summary>
        public ICredentials Credentials { private get; set; }

        /// <summary>
        /// Sets the name of the test container from which to run tests. (Optional)
        /// </summary>
        public string TestContainer {
            set {
                _testContainer = value;
                _testCase = AllTestCases;
            }
        }

        internal Runner(IAspClient client) {
            _client = client;
        }

        /// <summary>
        /// Creates a new AspUnitRunner.Runner instance.
        /// </summary>
        /// <returns>A new AspUnitRunner.Runner instance.</returns>
        public static Runner Create() {
            return Infrastructure.Ioc.ResolveRunner();
        }

        /// <summary>
        /// Sets the name of the test case to execute and its parent test container.
        /// </summary>
        /// <param name="testContainer">The name of the parent test container of the test case.</param>
        /// <param name="testCase">The name of the test case.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The test container is not specified.</exception>
        public void SetTestCase(string testContainer, string testCase) {
            if (IsSpecified(testCase, AllTestCases) && !IsSpecified(testContainer, AllTestContainers))
                throw new ArgumentOutOfRangeException("A test container must be specified for the test case.", "testContainer");

            _testContainer = testContainer;
            _testCase = testCase;
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
            return new NameValueCollection() {
                { "cboTestContainers", _testContainer },
                { "cboTestCases", _testCase },
                { "cmdRun", RunCommand }
            };
        }

        private static bool IsSpecified(string value, string defaultValue) {
            if (string.IsNullOrEmpty(value))
                return false;
            return value != defaultValue;
        }
    }
}
