using System;
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
        /// Sets the configuration and returns the current Runner object.
        /// </summary>
        /// <param name="configuration">The configuration object.</param>
        /// <returns>Returns the current Runner object.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// A test container must be specified if a test case is specified.
        /// </exception>
        public Runner WithConfiguration(Configuration configuration) {
            _client.Credentials = configuration.Credentials;
            SetTests(configuration.TestContainer, configuration.TestCase);
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

        private void SetTests(string testContainer, string testCase) {
            if (IsSpecified(testCase, AllTestCases) && !IsSpecified(testContainer, AllTestContainers))
                throw new ArgumentOutOfRangeException("A test container must be specified if a test case is specified.", "TestContainer");

            _testContainer = Normalize(testContainer, AllTestContainers);
            _testCase = Normalize(testCase, AllTestCases);
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
