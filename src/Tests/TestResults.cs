using NUnit.Framework;
using AspUnitRunner;
using AspUnitRunner.Core;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestResults {
        [Test]
        public void Constructor_should_set_properties_to_expected_values() {
            const int tests = 3;
            const int errors = 1;
            const int failures = 2;
            var details = new ResultDetail[] {
                new ResultDetail(ResultType.Error, "TestContainer.ErrorTest", "Microsoft VBScript runtime error (13): Type mismatch"),
                new ResultDetail(ResultType.Failure, "TestContainer.FailingTest1", "The assertion failed"),
                new ResultDetail(ResultType.Failure, "TestContainer.FailingTest2", "The test did not pass")
            };
            var html = FakeTestFormatter.FormatResults(tests, errors, failures, details);

            var results = new Results(tests, errors, failures, details, html);

            Assert.That(results.Tests, Is.EqualTo(tests));
            Assert.That(results.Errors, Is.EqualTo(errors));
            Assert.That(results.Failures, Is.EqualTo(failures));
            Assert.That(results.Details, Is.EqualTo(details));
            Assert.That(results.Html, Is.EqualTo(html));
        }
    }
}
