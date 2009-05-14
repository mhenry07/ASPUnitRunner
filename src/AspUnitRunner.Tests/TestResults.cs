using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AspUnitRunner;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestResults {
        [Test]
        public void Passing_tests_should_return_no_errors_or_failures() {
            Results results = new Results(FormatTestSummary(1, 0, 0));
            Assert.That(results.Errors, Is.EqualTo(0));
            Assert.That(results.Failures, Is.EqualTo(0));
        }

        [Test]
        public void Failing_test_should_return_a_failure() {
            Results results = new Results(FormatTestSummary(1, 0, 1));
            Assert.That(results.Failures, Is.EqualTo(1));
        }

        [Test]
        public void Erroneous_test_should_return_an_error() {
            Results results = new Results(FormatTestSummary(1, 1, 0));
            Assert.That(results.Errors, Is.EqualTo(1));
        }

        [Test]
        public void Should_return_expected_test_count() {
            Results results = new Results(FormatTestSummary(1, 0, 0));
            Assert.That(results.Tests, Is.EqualTo(1));
        }

        [Test]
        public void Should_return_expected_details() {
            string htmlTestResults = FormatTestSummary(1, 0, 0);
            Results results = new Results(htmlTestResults);
            Assert.That(results.Details, Is.EqualTo(htmlTestResults));
        }

        public static string FormatTestSummary(int tests, int errors, int failures) {
            return String.Format("<html><body><table><tr>Tests: {0}, Errors: {1}, Failures: {2}</tr></table></body></html>",
                tests, errors, failures);
        }
    }
}
