using System;
using NUnit.Framework;
using AspUnitRunner.Core;

namespace AspUnitRunner.Tests.Core {
    [TestFixture]
    public class TestResults {
        [Test]
        public void Successful_with_no_errors_or_failures_should_be_true() {
            var results = new Results {
                Tests = 1,
                Errors = 0,
                Failures = 0
            };

            Assert.That(results.Successful, Is.True);
        }

        [Test]
        public void Successful_with_an_error_should_be_false() {
            var results = new Results {
                Tests = 1,
                Errors = 1,
                Failures = 0
            };

            Assert.That(results.Successful, Is.False);
        }

        [Test]
        public void Successful_with_an_failure_should_be_false() {
            var results = new Results {
                Tests = 1,
                Errors = 0,
                Failures = 1
            };

            Assert.That(results.Successful, Is.False);
        }

        [Test]
        public void FormatDetails_with_empty_detail_should_return_empty_string() {
            var results = new Results {
                DetailList = new IResultDetail[] { }
            };

            Assert.That(results.FormatDetails(), Is.Empty);
        }

        [Test]
        public void FormatDetails_with_one_detail_should_return_expected_string() {
            var results = new Results {
                DetailList = new IResultDetail[] {
                    new ResultDetail(ResultType.Failure, "Container.TestCase", "Description")
                }
            };

            Assert.That(results.FormatDetails(),
                Is.EqualTo("Failure: Container.TestCase: Description"));
        }

        [Test]
        public void FormatDetails_with_two_details_should_return_expected_string() {
            var expectedFormat = "Failure: Container.Test1: First" + Environment.NewLine
                + "Error: Container.Test2: Second";
            var results = new Results {
                DetailList = new IResultDetail[] {
                    new ResultDetail(ResultType.Failure, "Container.Test1", "First"),
                    new ResultDetail(ResultType.Error, "Container.Test2", "Second")
                }
            };

            Assert.That(results.FormatDetails(), Is.EqualTo(expectedFormat));
        }

        [Test]
        public void FormatSummary_should_return_expected_string() {
            var results = new Results {
                Tests = 5,
                Errors = 1,
                Failures = 2
            };

            Assert.That(results.FormatSummary(),
                Is.EqualTo("Tests: 5, Errors: 1, Failures: 2"));
        }

        [Test]
        public void Format_should_return_expected_string() {
            var expectedFormat = "Failure: Container.Test1: First" + Environment.NewLine
                + "Error: Container.Test2: Second" + Environment.NewLine + Environment.NewLine
                + "Tests: 5, Errors: 1, Failures: 1" + Environment.NewLine;
            var results = new Results {
                Tests = 5,
                Errors = 1,
                Failures = 1,
                DetailList = new IResultDetail[] {
                    new ResultDetail(ResultType.Failure, "Container.Test1", "First"),
                    new ResultDetail(ResultType.Error, "Container.Test2", "Second")
                }
            };

            Assert.That(results.Format(), Is.EqualTo(expectedFormat));
        }
    }
}
