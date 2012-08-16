using System.Collections.Specialized;
using System.Net;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner;
using AspUnitRunner.Core;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestRunner {
        private const string TestContainerField = "_testContainer";
        private const string TestCaseField = "_testCase";

        private IAspClient _client;
        private IResultParser _resultParser;

        [SetUp]
        public void SetUp() {
            _client = MockRepository.GenerateMock<IAspClient>();
            _client.Stub(c =>
                    c.PostRequest(
                        Arg<string>.Is.Anything,
                        Arg<NameValueCollection>.Is.Anything))
                .Return(FakeTestFormatter.FormatSummary(1, 0, 0));
            _resultParser = MockRepository.GenerateStub<IResultParser>();
        }

        [Test]
        public void New_Runner_should_have_default_configuration() {
            var runner = CreateRunner();

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(Runner.AllTestContainers));
            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(Runner.AllTestCases));
            _client.AssertWasNotCalled(c => c.Credentials = Arg<ICredentials>.Is.NotNull);
        }

        [Test]
        public void Running_tests_should_return_expected_results() {
            var expectedResults = new Results();
            _resultParser.Stub(p => p.Parse(Arg<string>.Is.Anything))
                .Return(expectedResults);
            var runner = CreateRunner();

            var results = runner.Run("http://path/to/test-runner");
            Assert.That(results, Is.EqualTo(expectedResults));
        }

        [Test]
        public void Running_tests_should_post_request_to_expected_address_with_all_test_containers() {
            var expectedData = new NameValueCollection() {
                { "cboTestContainers", Runner.AllTestContainers },
                { "cboTestCases", Runner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner();
            var results = runner.Run("http://path/to/test-runner");

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg.Is("http://path/to/test-runner?UnitRunner=results"),
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Running_test_container_should_post_request_with_test_container() {
            const string testContainer = "TestContainer";
            var expectedData = new NameValueCollection() {
                { "cboTestContainers", testContainer },
                { "cboTestCases", Runner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithTestContainer(testContainer);
            var results = runner.Run("http://path/to/test-runner");

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Running_test_case_should_post_request_with_test_container_and_test_case() {
            const string testContainer = "TestContainer";
            const string testCase = "TestCase";
            var expectedData = new NameValueCollection() {
                { "cboTestContainers", testContainer },
                { "cboTestCases", testCase },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithTestContainerAndCase(testContainer, testCase);
            var results = runner.Run("http://path/to/test-runner");

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void WithCredentials_should_set_client_credentials() {
            var credentials = new NetworkCredential("username", "password");

            var runner = CreateRunner()
                .WithCredentials(credentials);

            _client.AssertWasCalled(c => c.Credentials = credentials);
        }

        [Test]
        public void WithEncoding_should_set_client_encoding() {
            var encoding = Encoding.UTF8;

            var runner = CreateRunner()
                .WithEncoding(encoding);

            _client.AssertWasCalled(c => c.Encoding = encoding);
        }

        [Test]
        public void WithTestContainerAndCase_with_test_case_for_all_containers_should_throw_exception() {
            var runner = CreateRunner();

            Assert.That(
                () => runner.WithTestContainerAndCase(Runner.AllTestContainers, "TestCase"),
                Throws.InstanceOf<System.ArgumentException>());
        }

        [Test]
        public void WithTestContainer_should_set_test_container() {
            const string testContainer = "TestContainer";
            var runner = CreateRunner()
                .WithTestContainer(testContainer);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(testContainer));
        }

        [Test]
        public void WithTestContainer_null_should_use_all_containers() {
            var runner = CreateRunner()
                .WithTestContainer(null);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(Runner.AllTestContainers));
        }

        [Test]
        public void WithTestContainer_empty_should_use_all_containers() {
            var runner = CreateRunner()
                .WithTestContainer("");

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(Runner.AllTestContainers));
        }

        [Test]
        public void WithTestContainerAndCase_should_set_test_container_and_test_case() {
            const string testContainer = "TestContainer";
            const string testCase = "TestCase";
            var runner = CreateRunner()
                .WithTestContainerAndCase(testContainer, testCase);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(testContainer));
            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(testCase));
        }

        [Test]
        public void WithTestContainerAndCase_with_null_test_case_should_use_all_test_cases() {
            var runner = CreateRunner()
                .WithTestContainerAndCase("TestContainer", null);

            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(Runner.AllTestCases));
        }

        [Test]
        public void WithTestContainerAndCase_with_empty_test_case_should_use_all_test_cases() {
            var runner = CreateRunner()
                .WithTestContainerAndCase("TestContainer", "");

            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(Runner.AllTestCases));
        }

        private Runner CreateRunner() {
            return new Runner(_client, _resultParser);
        }
    }
}
