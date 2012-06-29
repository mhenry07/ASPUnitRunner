using System;
using System.Net;
using NUnit.Framework;
using Rhino.Mocks;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestRunner {
        private IAspProxy _proxy;

        [SetUp]
        public void Setup() {
            _proxy = MockRepository.GenerateStub<IAspProxy>();
            _proxy.Stub(x => x.GetTestResults("", "", null))
                .IgnoreArguments()
                .Return(TestResults.FormatTestSummary(1, 0, 0));
        }

        [Test]
        public void Running_tests_should_return_results() {
            Runner runner = new Runner("", _proxy);
            Assert.That(runner.Run(""), Is.TypeOf<Results>());
        }

        [Test]
        public void Should_pass_expected_uri_to_proxy() {
            Runner runner = new Runner("http://path/to/test-runner", _proxy);
            Results results = runner.Run("");
            _proxy.AssertWasCalled(x => x.GetTestResults(Arg.Is("http://path/to/test-runner?UnitRunner=results"), Arg<string>.Is.Anything, Arg<ICredentials>.Is.Anything));
        }

        [Test]
        public void Should_pass_expected_data_to_proxy() {
            Runner runner = new Runner("http://path/to/test-runner", _proxy);
            Results results = runner.Run("TestContainer");
            _proxy.AssertWasCalled(x => x.GetTestResults(Arg<string>.Is.Anything, Arg.Is("cboTestContainers=TestContainer&cboTestCases=All+Test+Cases&cmdRun=Run+Tests"), Arg<ICredentials>.Is.Anything));
        }

        [Test]
        public void Should_pass_credentials_to_proxy() {
            ICredentials credentials = new NetworkCredential("username", "password");
            Runner runner = new Runner("", credentials, _proxy);
            Results results = runner.Run("");
            _proxy.AssertWasCalled(x => x.GetTestResults(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg.Is(credentials)));
        }
    }
}
