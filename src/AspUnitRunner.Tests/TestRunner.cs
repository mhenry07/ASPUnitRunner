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
    }
}
