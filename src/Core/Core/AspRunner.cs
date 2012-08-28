using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace AspUnitRunner.Core {
    internal class AspRunner : IRunner {
        internal const string AllTestContainers = "All Test Containers";
        internal const string AllTestCases = "All Test Cases";

        private const string RunCommand = "Run Tests";
        private const string ResultsQueryString = "?UnitRunner=results";

        private readonly IAspClient _client;
        private readonly IResultParser _resultParser;

        private string _address;
        private string _testContainer = AllTestContainers;
        private string _testCase = AllTestCases;

        internal AspRunner(IAspClient client, IResultParser resultParser) {
            _client = client;
            _resultParser = resultParser;
        }

        internal IRunner WithAddress(string address) {
            _address = address;
            return this;
        }

        public IRunner WithCredentials(ICredentials credentials) {
            _client.Credentials = credentials;
            return this;
        }

        public IRunner WithEncoding(Encoding encoding) {
            _client.Encoding = encoding;
            return this;
        }

        public IRunner WithTestContainer(string testContainer) {
            return WithTestContainerAndCase(testContainer, AllTestCases);
        }

        public IRunner WithTestContainerAndCase(string testContainer, string testCase) {
            if (IsSpecified(testCase, AllTestCases) && !IsSpecified(testContainer, AllTestContainers))
                throw new ArgumentException("A test container must be specified if a test case is specified.", "testContainer");

            _testContainer = Normalize(testContainer, AllTestContainers);
            _testCase = Normalize(testCase, AllTestCases);

            return this;
        }

        public Results Run() {
            var htmlResults = _client.PostRequest(FormatUrl(_address), GetPostData());
            return _resultParser.Parse(htmlResults);
        }

        private string FormatUrl(string address) {
            return address + ResultsQueryString;
        }

        private NameValueCollection GetPostData() {
            return new NameValueCollection {
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
