using System.Net;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestRunner {
        private IAspProxy _proxy;

        [SetUp]
        public void Setup() {
            _proxy = MockRepository.GenerateStub<IAspProxy>();
            _proxy.Stub(proxy => proxy.GetTestResults("", "", null))
                .IgnoreArguments()
                .Return(FakeTestFormatter.FormatSummary(1, 0, 0));
        }

        [Test]
        public void Running_tests_should_return_results() {
            var runner = new Runner("", _proxy);
            Assert.That(runner.Run(""), Is.TypeOf<Results>());
        }

        [Test]
        public void Running_tests_should_pass_expected_arguments_to_proxy() {
            var runner = new Runner("http://path/to/test-runner", _proxy);
            var results = runner.Run("TestContainer");
            _proxy.AssertWasCalled(proxy =>
                proxy.GetTestResults(
                    Arg.Is("http://path/to/test-runner?UnitRunner=results"),
                    Arg.Is("cboTestContainers=TestContainer&cboTestCases=All+Test+Cases&cmdRun=Run+Tests"),
                    Arg<ICredentials>.Is.Null));
        }

        [Test]
        public void Running_tests_with_credentials_should_pass_credentials_to_proxy() {
            ICredentials credentials = new NetworkCredential("username", "password");
            var runner = new Runner("", credentials, _proxy);
            var results = runner.Run("");
            _proxy.AssertWasCalled(proxy =>
                proxy.GetTestResults(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg.Is(credentials)));
        }
    }
}
