using System;
using System.Net;
using System.Collections.Specialized;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public class Runner {
        public const string AllTestContainers = "All Test Containers";
        public const string AllTestCases = "All Test Cases";
        private const string RunCommand = "Run Tests";
        private const string ResultsQueryString = "?UnitRunner=results";

        private readonly IAspClient _client;
        private string _testContainer = AllTestContainers;
        private string _testCase = AllTestCases;

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
        /// Sets the network credentials used to authenticate the request
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="credentials">The network credentials.</param>
        /// <returns>The current Runner object.</returns>
        public Runner WithCredentials(ICredentials credentials) {
            _client.Credentials = credentials;
            return this;
        }

        /// <summary>
        /// Sets the name of the test container from which to run tests
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="testContainer">The test container.</param>
        /// <returns>The current Runner object.</returns>
        public Runner WithTestContainer(string testContainer) {
            return WithTestContainerAndCase(testContainer, AllTestCases);
        }

        /// <summary>
        /// Sets the name of the test container and test case to execute
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="testContainer">The test container containing the test case.</param>
        /// <param name="testCase">The test case to execute.</param>
        /// <returns>The current Runner object.</returns>
        public Runner WithTestContainerAndCase(string testContainer, string testCase) {
            if (IsSpecified(testCase, AllTestCases) && !IsSpecified(testContainer, AllTestContainers))
                throw new ArgumentException("A test container must be specified if a test case is specified.", "testContainer");

            _testContainer = Normalize(testContainer, AllTestContainers);
            _testCase = Normalize(testCase, AllTestCases);

            return this;
        }

        /// <summary>
        /// Runs ASPUnit tests and returns results.
        /// </summary>
        /// <param name="address">The URL for the ASPUnit tests.</param>
        /// <returns>An AspUnitRunner.Results containing the test results.</returns>
        public Results Run(string address) {
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

        private static string Normalize(string value, string defaultValue) {
            if (string.IsNullOrEmpty(value))
                return defaultValue;
            return value;
        }
    }
}
