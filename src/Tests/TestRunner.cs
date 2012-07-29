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
                    c.GetTestResults(
                        Arg<string>.Is.Anything,
                        Arg<NameValueCollection>.Is.Anything,
                        Arg<ICredentials>.Is.Anything))
                .Return(FakeTestFormatter.FormatSummary(1, 0, 0));
        }

        [Test]
        public void Running_tests_should_return_results() {
            var runner = new Runner(_client);
            Assert.That(runner.Run("", ""), Is.InstanceOf<Results>());
        }

        [Test]
        public void Running_tests_should_pass_expected_arguments_to_client() {
            var expectedData = new NameValueCollection() {
                { "cboTestContainers", "TestContainer" },
                { "cboTestCases", "All Test Cases" },
                { "cmdRun", "Run Tests"}
            };

            var runner = new Runner(_client);
            var results = runner.Run("http://path/to/test-runner", "TestContainer");

            _client.AssertWasCalled(c =>
                c.GetTestResults(
                    Arg.Is("http://path/to/test-runner?UnitRunner=results"),
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData)),
                    Arg<ICredentials>.Is.Null));
        }

        [Test]
        public void Running_tests_with_credentials_should_pass_credentials_to_client() {
            var credentials = new NetworkCredential("username", "password");

            var runner = new Runner(_client);
            var results = runner.Run("", "", credentials);
            _client.AssertWasCalled(c =>
                c.GetTestResults(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Is.Anything,
                    Arg.Is(credentials)));
        }
    }
}
