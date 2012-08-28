using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public class Runner : IRunner {
        internal const string AllTestContainers = "All Test Containers";
        internal const string AllTestCases = "All Test Cases";

        private const string RunCommand = "Run Tests";
        private const string ResultsQueryString = "?UnitRunner=results";

        private readonly IAspClient _client;
        private readonly IResultParser _resultParser;

        private string _address;
        private string _testContainer = AllTestContainers;
        private string _testCase = AllTestCases;

        internal Runner(IAspClient client, IResultParser resultParser) {
            _client = client;
            _resultParser = resultParser;
        }

        /// <summary>
        /// Creates a new AspUnitRunner.Runner instance.
        /// </summary>
        /// <param name="address">The URL for the ASPUnit tests.</param>
        /// <returns>A new AspUnitRunner.Runner instance.</returns>
        public static IRunner Create(string address) {
            return Infrastructure.Ioc.ResolveRunner()
                .WithAddress(address);
        }

        internal IRunner WithAddress(string address) {
            _address = address;
            return this;
        }

        /// <summary>
        /// Sets the network credentials used to authenticate the request
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="credentials">The network credentials.</param>
        /// <returns>The current IRunner object.</returns>
        public IRunner WithCredentials(ICredentials credentials) {
            _client.Credentials = credentials;
            return this;
        }

        /// <summary>
        /// Sets the default encoding used to encode the request and decode the
        /// response and returns the current Runner object.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The current IRunner object.</returns>
        /// <remarks>A charset in the response headers will take precedence.</remarks>
        public IRunner WithEncoding(Encoding encoding) {
            _client.Encoding = encoding;
            return this;
        }

        /// <summary>
        /// Sets the name of the test container from which to run tests
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="testContainer">The test container.</param>
        /// <returns>The current IRunner object.</returns>
        public IRunner WithTestContainer(string testContainer) {
            return WithTestContainerAndCase(testContainer, AllTestCases);
        }

        /// <summary>
        /// Sets the name of the test container and test case to execute
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="testContainer">The test container containing the test case.</param>
        /// <param name="testCase">The test case to execute.</param>
        /// <returns>The current IRunner object.</returns>
        public IRunner WithTestContainerAndCase(string testContainer, string testCase) {
            if (IsSpecified(testCase, AllTestCases) && !IsSpecified(testContainer, AllTestContainers))
                throw new ArgumentException("A test container must be specified if a test case is specified.", "testContainer");

            _testContainer = Normalize(testContainer, AllTestContainers);
            _testCase = Normalize(testCase, AllTestCases);

            return this;
        }

        /// <summary>
        /// Runs ASPUnit tests and returns results.
        /// </summary>
        /// <returns>An AspUnitRunner.Results containing the test results.</returns>
        public Results Run() {
            var htmlResults = _client.PostRequest(FormatUrl(_address), GetPostData());
            return _resultParser.Parse(htmlResults);
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
