using System.Collections.Specialized;
using System.Net;
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

        [SetUp]
        public void SetUp() {
            _client = MockRepository.GenerateMock<IAspClient>();
            _client.Stub(c =>
                    c.PostRequest(
                        Arg<string>.Is.Anything,
                        Arg<NameValueCollection>.Is.Anything))
                .Return(FakeTestFormatter.FormatSummary(1, 0, 0));
        }

        [Test]
        public void New_Runner_should_have_default_configuration() {
            var runner = new Runner(_client);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(Runner.AllTestContainers));
            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(Runner.AllTestCases));
            _client.AssertWasNotCalled(c => c.Credentials = Arg<ICredentials>.Is.NotNull);
        }

        [Test]
        public void Running_tests_should_return_results() {
            var runner = new Runner(_client);
            var results = runner.Run("http://path/to/test-runner");
            Assert.That(results, Is.InstanceOf<Results>());
        }

        [Test]
        public void Running_tests_should_post_request_to_expected_address_with_all_test_containers() {
            var expectedData = new NameValueCollection() {
                { "cboTestContainers", Runner.AllTestContainers },
                { "cboTestCases", Runner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = new Runner(_client);
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

            var runner = new Runner(_client)
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

            var runner = new Runner(_client)
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

            var runner = new Runner(_client)
                .WithCredentials(credentials);

            _client.AssertWasCalled(c => c.Credentials = credentials);
        }

        [Test]
        public void WithTestContainerAndCase_with_test_case_for_all_containers_should_throw_exception() {
            var runner = new Runner(_client);

            Assert.That(
                () => runner.WithTestContainerAndCase(Runner.AllTestContainers, "TestCase"),
                Throws.InstanceOf<System.ArgumentException>());
        }

        [Test]
        public void WithTestContainer_should_set_test_container() {
            const string testContainer = "TestContainer";
            var runner = new Runner(_client)
                .WithTestContainer(testContainer);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(testContainer));
        }

        [Test]
        public void WithTestContainer_null_should_use_all_containers() {
            var runner = new Runner(_client)
                .WithTestContainer(null);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(Runner.AllTestContainers));
        }

        [Test]
        public void WithTestContainer_empty_should_use_all_containers() {
            var runner = new Runner(_client)
                .WithTestContainer("");

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(Runner.AllTestContainers));
        }

        [Test]
        public void WithTestContainerAndCase_should_set_test_container_and_test_case() {
            const string testContainer = "TestContainer";
            const string testCase = "TestCase";
            var runner = new Runner(_client)
                .WithTestContainerAndCase(testContainer, testCase);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(testContainer));
            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(testCase));
        }

        [Test]
        public void WithTestContainerAndCase_with_null_test_case_should_use_all_test_cases() {
            var runner = new Runner(_client)
                .WithTestContainerAndCase("TestContainer", null);

            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(Runner.AllTestCases));
        }

        [Test]
        public void WithTestContainerAndCase_with_empty_test_case_should_use_all_test_cases() {
            var runner = new Runner(_client)
                .WithTestContainerAndCase("TestContainer", "");

            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(Runner.AllTestCases));
        }
    }
}
