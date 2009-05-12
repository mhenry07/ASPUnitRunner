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
            Runner runner = new Runner();
            Assert.That(runner.Run(), Is.TypeOf<Results>());
        }

        [Test]
        public void Passing_tests_should_return_no_errors_or_failures() {
            Runner runner = new Runner();
            Results results = runner.Run();
            Assert.That(results.Errors, Is.EqualTo(0));
            Assert.That(results.Failures, Is.EqualTo(0));
        }

        [Test]
        public void Failing_test_should_return_a_failure() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = "<html><body><table><tr>Tests: 1, Errors: 0, Failures: 1</tr></table></body></html>";

            Runner runner = new Runner(fakeProxy);
            Results results = runner.Run();
            Assert.That(results.Failures, Is.EqualTo(1));
        }

        [Test]
        public void Erroneous_test_should_return_an_error() {
            FakeProxy fakeProxy = new FakeProxy();
            fakeProxy.HtmlResults = "<html><body><table><tr>Tests: 1, Errors: 1, Failures: 0</tr></table></body></html>";

            Runner runner = new Runner(fakeProxy);
            Results results = runner.Run();
            Assert.That(results.Errors, Is.EqualTo(1));
        }
    }
}
