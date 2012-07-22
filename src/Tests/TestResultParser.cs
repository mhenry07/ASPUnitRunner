using System;
using NUnit.Framework;
using AspUnitRunner.Core;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestResultParser {
        [Test]
        public void Parse_passing_tests_should_return_no_errors_or_failures() {
            var results = ResultParser.Parse(FormatTestSummary(1, 0, 0));
            Assert.That(results.Errors, Is.EqualTo(0));
            Assert.That(results.Failures, Is.EqualTo(0));
        }

        [Test]
        public void Parse_failing_test_should_return_a_failure() {
            var results = ResultParser.Parse(FormatTestSummary(1, 0, 1));
            Assert.That(results.Failures, Is.EqualTo(1));
        }

        [Test]
        public void Parse_test_error_should_return_an_error() {
            var results = ResultParser.Parse(FormatTestSummary(1, 1, 0));
            Assert.That(results.Errors, Is.EqualTo(1));
        }

        [Test]
        public void Parse_single_test_should_return_expected_one_test() {
            var results = ResultParser.Parse(FormatTestSummary(1, 0, 0));
            Assert.That(results.Tests, Is.EqualTo(1));
        }

        [Test]
        public void Parse_should_return_expected_details() {
            var htmlTestResults = FormatTestSummary(1, 0, 0);
            var results = ResultParser.Parse(htmlTestResults);
            Assert.That(results.Details, Is.EqualTo(htmlTestResults));
        }

        [Test]
        public void Parse_invalid_results_should_throw_format_exception() {
            Assert.That(
                delegate { ResultParser.Parse(""); },
                Throws.InstanceOf<FormatException>());
        }

        private string FormatTestSummary(int tests, int errors, int failures) {
            return FakeTestFormatter.FormatSummary(tests, errors, failures);
        }
    }
}
