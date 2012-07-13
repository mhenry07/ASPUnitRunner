using NUnit.Framework;
using AspUnitRunner;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestResults {
        [Test]
        public void Passing_tests_should_return_no_errors_or_failures() {
            var results = new Results(FormatTestSummary(1, 0, 0));
            Assert.That(results.Errors, Is.EqualTo(0));
            Assert.That(results.Failures, Is.EqualTo(0));
        }

        [Test]
        public void Failing_test_should_return_a_failure() {
            var results = new Results(FormatTestSummary(1, 0, 1));
            Assert.That(results.Failures, Is.EqualTo(1));
        }

        [Test]
        public void Erroneous_test_should_return_an_error() {
            var results = new Results(FormatTestSummary(1, 1, 0));
            Assert.That(results.Errors, Is.EqualTo(1));
        }

        [Test]
        public void Should_return_expected_test_count() {
            var results = new Results(FormatTestSummary(1, 0, 0));
            Assert.That(results.Tests, Is.EqualTo(1));
        }

        [Test]
        public void Should_return_expected_details() {
            var htmlTestResults = FormatTestSummary(1, 0, 0);
            var results = new Results(htmlTestResults);
            Assert.That(results.Details, Is.EqualTo(htmlTestResults));
        }

        private string FormatTestSummary(int tests, int errors, int failures) {
            return FakeTestFormatter.FormatSummary(tests, errors, failures);
        }
    }
}
