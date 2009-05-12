using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using AspUnitRunner;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestRunner {
        [Test]
        public void Should_run_tests() {
            Runner runner = new Runner();
            Assert.That(runner.Run(), Is.True);
        }
    }
}
