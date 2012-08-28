using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner.Core;
using AspUnitRunner.Core.Html;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Unit.Core {
    // These tests for ResultParser are more integration tests than unit
    // tests.
    // For the most part, we're using the actual collaborators for these
    // tests since the test doubles for all HtmlDocument, HtmlElement and
    // HtmlCollection objects used by ResultParser would be almost more
    // complex than the actual implementations (which do have unit tests).
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
        public void Parse_passing_test_should_return_empty_detail_list() {
            var details = new IResultDetail[] { };
            var htmlTestResults = FakeTestFormatter.FormatResults(1, 0, 0, details);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.DetailList,
                Is.InstanceOf<IEnumerable<IResultDetail>>().And.Empty);
        }

        [Test]
        public void Parse_erroneous_test_should_return_expected_detail_list() {
            var details = new List<IResultDetail> {
                new ResultDetail(ResultType.Error, "TestContainer.TestCase", "Error description")
            };
            var htmlTestResults = FakeTestFormatter.FormatResults(1, 1, 0, details);
            var parser = CreateResultParser(htmlTestResults);

            var results = parser.Parse(htmlTestResults);
            Assert.That(results.DetailList, Is.EqualTo(details)
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

        private class ResultDetailEqualityComparer : IEqualityComparer<IResultDetail> {
            public bool Equals(IResultDetail x, IResultDetail y) {
                return x.Type.Equals(y.Type)
                    && x.Name.Equals(y.Name)
                    && x.Description.Equals(y.Description);
            }

            public int GetHashCode(IResultDetail obj) {
                throw new NotImplementedException();
            }
        }
    }
}
