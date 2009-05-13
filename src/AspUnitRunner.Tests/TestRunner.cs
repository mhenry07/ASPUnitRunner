using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AspUnitRunner;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestRunner {
        [Test]
        public void Running_tests_should_return_results() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = FormatTestSummary(1, 0, 0);
            Runner runner = new Runner("", fakeProxy);
            Assert.That(runner.Run(""), Is.TypeOf<Results>());
        }

        [Test]
        public void Should_pass_expected_uri_to_proxy() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = FormatTestSummary(1, 0, 0);
            Runner runner = new Runner("http://path/to/test-runner", fakeProxy);
            Results results = runner.Run("");
            Assert.That(fakeProxy.Uri, Is.EqualTo("http://path/to/test-runner?UnitRunner=results"));
        }

        [Test]
        public void Should_pass_expected_data_to_proxy() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = FormatTestSummary(1, 0, 0);
            Runner runner = new Runner("http://path/to/test-runner", fakeProxy);
            Results results = runner.Run("TestContainer");
            Assert.That(fakeProxy.PostData, Is.EqualTo("cboTestContainers=TestContainer&cboTestCases=All+Test+Cases&cmdRun=Run+Tests"));
        }

        public static string FormatTestSummary(int tests, int errors, int failures) {
            return String.Format("<html><body><table><tr>Tests: {0}, Errors: {1}, Failures: {2}</tr></table></body></html>",
                tests, errors, failures);
        }
    }
}
