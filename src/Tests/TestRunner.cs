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
                { "cboTestContainers", "All Test Containers" },
                { "cboTestCases", "All Test Cases" },
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
                { "cboTestCases", "All Test Cases" },
                { "cmdRun", "Run Tests"}
            };

            var runner = new Runner(_client);
            runner.TestContainer = testContainer;
            var results = runner.Run("http://path/to/test-runner");

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Running_test_case_should_post_request_with_test_case() {
            const string testContainer = "TestContainer";
            const string testCase = "TestCase";
            var expectedData = new NameValueCollection() {
                { "cboTestContainers", testContainer },
                { "cboTestCases", testCase },
                { "cmdRun", "Run Tests"}
            };

            var runner = new Runner(_client);
            runner.TestContainer = testContainer;
            runner.TestCase = testCase;
            var results = runner.Run("http://path/to/test-runner");

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Running_test_case_without_test_container_should_throw_exception() {
            var runner = new Runner(_client);
            runner.TestCase = "OrphanTestCase";

            Assert.That(
                delegate { runner.Run("http://path/to/test-runner"); },
                Throws.InvalidOperationException);
        }

        [Test]
        public void Running_tests_with_credentials_should_set_client_credentials() {
            var credentials = new NetworkCredential("username", "password");

            var runner = new Runner(_client);
            runner.Credentials = credentials;
            var results = runner.Run("https://path/to/test-runner");

            _client.AssertWasCalled(c => c.Credentials = credentials);
        }

        [Test]
        public void TestContainer_get_default_should_return_All_Test_Containers() {
            var runner = new Runner(_client);
            Assert.That(runner.TestContainer, Is.EqualTo("All Test Containers"));
        }

        [Test]
        public void TestContainer_get_assigned_should_return_assigned_value() {
            const string testContainer = "Test Container";

            var runner = new Runner(_client);
            runner.TestContainer = testContainer;
            Assert.That(runner.TestContainer, Is.EqualTo(testContainer));
        }

        [Test]
        public void TestCase_get_default_should_return_All_Test_Cases() {
            var runner = new Runner(_client);
            Assert.That(runner.TestCase, Is.EqualTo("All Test Cases"));
        }

        [Test]
        public void TestCase_get_assigned_should_return_assigned_value() {
            const string testCase = "TestCase";

            var runner = new Runner(_client);
            runner.TestCase = testCase;
            Assert.That(runner.TestCase, Is.EqualTo(testCase));
        }
    }
}
