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
            Runner runner = new Runner();
            Results results = runner.Run();
            Assert.That(results.Failures, Is.EqualTo(1));
        }
    }
}
