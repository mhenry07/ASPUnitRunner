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
                .WithConfiguration(new Configuration { TestContainer = testContainer });
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
                .WithConfiguration(new Configuration {
                    TestContainer = testContainer,
                    TestCase = testCase
                });
            var results = runner.Run("http://path/to/test-runner");

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void WithConfiguration_with_credentials_should_set_client_credentials() {
            var credentials = new NetworkCredential("username", "password");

            var runner = new Runner(_client)
                .WithConfiguration(new Configuration { Credentials = credentials });

            _client.AssertWasCalled(c => c.Credentials = credentials);
        }

        [Test]
        public void WithCredentials_should_set_client_credentials() {
            var credentials = new NetworkCredential("username", "password");

            var runner = new Runner(_client)
                .WithCredentials(credentials);

            _client.AssertWasCalled(c => c.Credentials = credentials);
        }

        [Test]
        public void WithConfiguration_with_test_case_for_all_containers_should_throw_exception() {
            var runner = new Runner(_client);

            Assert.That(
                () => runner.WithConfiguration(new Configuration { TestContainer = Runner.AllTestContainers, TestCase = "TestCase" }),
                Throws.InstanceOf<System.ArgumentOutOfRangeException>());
        }

        [Test]
        public void WithConfiguration_with_null_container_should_use_all_containers() {
            var runner = new Runner(_client)
                .WithConfiguration(new Configuration { TestContainer = null });

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(Runner.AllTestContainers));
        }

        [Test]
        public void WithConfiguration_with_empty_container_should_use_all_containers() {
            var runner = new Runner(_client)
                .WithConfiguration(new Configuration { TestContainer = "" });

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(Runner.AllTestContainers));
        }

        [Test]
        public void WithConfiguration_with_null_test_case_should_use_all_test_cases() {
            var runner = new Runner(_client)
                .WithConfiguration(new Configuration { TestCase = null });

            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(Runner.AllTestCases));
        }

        [Test]
        public void WithConfiguration_with_empty_test_case_should_use_all_test_cases() {
            var runner = new Runner(_client)
                .WithConfiguration(new Configuration { TestCase = "" });

            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(Runner.AllTestCases));
        }
    }
}
