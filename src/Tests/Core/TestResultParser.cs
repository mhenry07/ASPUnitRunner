using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner.Core;
using AspUnitRunner.Core.Html;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Core {
    [TestFixture]
    public class TestResultParser {
        private IHtmlDocumentFactory _htmlDocumentFactory;

        [SetUp]
        public void SetUp() {
            _htmlDocumentFactory = MockRepository.GenerateStub<IHtmlDocumentFactory>();
        }

        [Test]
        public void Parse_passing_tests_should_return_no_errors_or_failures() {
            var htmlTestResults = FormatTestSummary(1, 0, 0);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.Errors, Is.EqualTo(0));
            Assert.That(results.Failures, Is.EqualTo(0));
        }

        [Test]
        public void Parse_failing_test_should_return_a_failure() {
            var htmlTestResults = FormatTestSummary(1, 0, 1);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.Failures, Is.EqualTo(1));
        }

        [Test]
        public void Parse_test_error_should_return_an_error() {
            var htmlTestResults = FormatTestSummary(1, 1, 0);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.Errors, Is.EqualTo(1));
        }

        [Test]
        public void Parse_single_test_should_return_expected_one_test() {
            var htmlTestResults = FormatTestSummary(1, 0, 0);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.Tests, Is.EqualTo(1));
        }

        [Test]
        public void Parse_should_return_expected_html() {
            var htmlTestResults = FormatTestSummary(1, 0, 0);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.Html, Is.EqualTo(htmlTestResults));
        }

        [Test]
        public void Parse_passing_test_should_return_empty_details() {
            var details = new ResultDetail[] { };
            var htmlTestResults = FakeTestFormatter.FormatResults(1, 0, 0, details);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.Details,
                Is.InstanceOf<IEnumerable<ResultDetail>>().And.Empty);
        }

        [Test]
        public void Parse_erroneous_test_should_return_expected_detail() {
            var details = new List<ResultDetail> {
                new ResultDetail(ResultType.Error, "TestContainer.TestCase", "Error description")
            };
            var htmlTestResults = FakeTestFormatter.FormatResults(1, 1, 0, details);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.Details, Is.EqualTo(details)
                .Using(new ResultDetailEqualityComparer()));
        }

        [Test]
        public void Parse_invalid_results_should_throw_format_exception() {
            var parser = CreateResultParser("");

            Assert.That(
                () => parser.Parse(""),
                Throws.InstanceOf<FormatException>());
        }

        private string FormatTestSummary(int tests, int errors, int failures) {
            return FakeTestFormatter.FormatSummary(tests, errors, failures);
        }

        private ResultParser CreateResultParser(string htmlTestResults) {
            _htmlDocumentFactory.Stub(f => f.Create(htmlTestResults))
                .Return(new HtmlDocument(htmlTestResults));
            return new ResultParser(_htmlDocumentFactory);
        }

        private class ResultDetailEqualityComparer : IEqualityComparer<ResultDetail> {
            public bool Equals(ResultDetail x, ResultDetail y) {
                return x.Type.Equals(y.Type)
                    && x.Name.Equals(y.Name)
                    && x.Description.Equals(y.Description);
            }

            public int GetHashCode(ResultDetail obj) {
                throw new NotImplementedException();
            }
        }
    }
}
