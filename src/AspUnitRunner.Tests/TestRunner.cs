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
        public void Passing_tests_should_return_no_errors_or_failures() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = FormatTestSummary(1, 0, 0);

            Runner runner = new Runner("", fakeProxy);
            Results results = runner.Run("");
            Assert.That(results.Errors, Is.EqualTo(0));
            Assert.That(results.Failures, Is.EqualTo(0));
        }

        [Test]
        public void Failing_test_should_return_a_failure() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = FormatTestSummary(1, 0, 1);

            Runner runner = new Runner("", fakeProxy);
            Results results = runner.Run("");
            Assert.That(results.Failures, Is.EqualTo(1));
        }

        [Test]
        public void Erroneous_test_should_return_an_error() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = FormatTestSummary(1, 1, 0);

            Runner runner = new Runner("", fakeProxy);
            Results results = runner.Run("");
            Assert.That(results.Errors, Is.EqualTo(1));
        }

        [Test]
        public void Should_pass_expected_uri_to_proxy() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = FormatTestSummary(1, 0, 0);
            Runner runner = new Runner("http://path/to/test-runner", fakeProxy);
            Results results = runner.Run("");
            Assert.That(fakeProxy.Url, Is.EqualTo("http://path/to/test-runner?UnitRunner=results"));
        }

        [Test]
        public void Should_pass_expected_data_to_proxy() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = FormatTestSummary(1, 0, 0);
            Runner runner = new Runner("http://path/to/test-runner", fakeProxy);
            Results results = runner.Run("TestContainer");
            Assert.That(fakeProxy.PostData, Is.EqualTo("cboTestContainers=TestContainer&cboTestCases=All+Test+Cases&cmdRun=Run+Tests"));
        }

        private string FormatTestSummary(int tests, int errors, int failures) {
            return String.Format("<html><body><table><tr>Tests: {0}, Errors: {1}, Failures: {2}</tr></table></body></html>",
                tests, errors, failures);
        }
    }
}
