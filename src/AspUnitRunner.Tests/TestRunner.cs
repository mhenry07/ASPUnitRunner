using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AspUnitRunner;
using System.Net;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestRunner {
        private FakeProxy _fakeProxy;

        [SetUp]
        public void Setup() {
            _fakeProxy = new FakeProxy();
            _fakeProxy.HtmlResults = FormatTestSummary(1, 0, 0);
        }

        [Test]
        public void Running_tests_should_return_results() {
            Runner runner = new Runner("", _fakeProxy);
            Assert.That(runner.Run(""), Is.TypeOf<Results>());
        }

        [Test]
        public void Should_pass_expected_uri_to_proxy() {
            Runner runner = new Runner("http://path/to/test-runner", _fakeProxy);
            Results results = runner.Run("");
            Assert.That(_fakeProxy.Uri, Is.EqualTo("http://path/to/test-runner?UnitRunner=results"));
        }

        [Test]
        public void Should_pass_expected_data_to_proxy() {
            Runner runner = new Runner("http://path/to/test-runner", _fakeProxy);
            Results results = runner.Run("TestContainer");
            Assert.That(_fakeProxy.PostData, Is.EqualTo("cboTestContainers=TestContainer&cboTestCases=All+Test+Cases&cmdRun=Run+Tests"));
        }

        [Test]
        public void Should_pass_credentials_to_proxy() {
            ICredentials credentials = new NetworkCredential("username", "password");
            Runner runner = new Runner("", credentials, _fakeProxy);
            Results results = runner.Run("");
            Assert.That(_fakeProxy.Credentials, Is.EqualTo(credentials));
        }

        public static string FormatTestSummary(int tests, int errors, int failures) {
            return String.Format("<html><body><table><tr>Tests: {0}, Errors: {1}, Failures: {2}</tr></table></body></html>",
                tests, errors, failures);
        }
    }
}
