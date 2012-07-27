using System.Collections.Generic;
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
            _client = MockRepository.GenerateStub<IAspClient>();
            _client.Stub(c => c.GetTestResults("", null, null))
                .IgnoreArguments()
                .Return(FakeTestFormatter.FormatSummary(1, 0, 0));
        }

        [Test]
        public void Running_tests_should_return_results() {
            var runner = new Runner(_client);
            Assert.That(runner.Run("", ""), Is.TypeOf<Results>());
        }

        [Test]
        public void Running_tests_should_pass_expected_arguments_to_client() {
            var runner = new Runner(_client);
            var results = runner.Run("http://path/to/test-runner", "TestContainer");
            _client.AssertWasCalled(c =>
                c.GetTestResults(
                    Arg.Is("http://path/to/test-runner?UnitRunner=results"),
                    Arg.Is(new KeyValuePair<string, string>[] {
                        new KeyValuePair<string, string>("cboTestContainers", "TestContainer"),
                        new KeyValuePair<string, string>("cboTestCases", "All Test Cases"),
                        new KeyValuePair<string, string>("cmdRun", "Run Tests")
                    }),
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
                    Arg<IEnumerable<KeyValuePair<string, string>>>.Is.Anything,
                    Arg.Is(credentials)));
        }
    }
}
