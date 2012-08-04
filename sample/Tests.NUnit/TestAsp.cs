﻿using System.Text;
using System.Threading;
using NUnit.Framework;
using AspUnitRunner;

namespace AspUnitRunner.Sample.Tests.NUnit {
    [TestFixture]
    public class TestAsp {
        // set the URL for your ASPUnit tests
        private const string AspTestUrl = "http://localhost:54831/tests/Default.asp";
        // set the site name as configured in IIS Express (defaults to name of sample web project: AspUnitRunner.Sample.Web)
        private const string AspSiteName = "AspUnitRunner.Sample.Web";

        private IisExpressServer _iisServer;

        [Test]
        public void TestCase(
            // set ASPUnit test containers here
            [Values("CalculatorTest", "StringUtilityTest", "FailureTest")] string testContainer
            ) {
            var runner = Runner.Create()
                .WithEncoding(Encoding.UTF8)
                .WithTestContainer(testContainer);
            var results = runner.Run(AspTestUrl);

            // Note: results.Html can generate a long HTML string which NUnit doesn't format very well
            Assert.That(results.Errors, Is.EqualTo(0), results.Html);
            Assert.That(results.Failures, Is.EqualTo(0), results.Html);
            if (results.Tests == 0)
                Assert.Inconclusive("0 tests were run");
        }

        [TestFixtureSetUp]
        public void TestFixtureSetUp() {
            _iisServer = new IisExpressServer(AspSiteName);
            var thread = new Thread(_iisServer.Start) { IsBackground = true };
            thread.Start();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown() {
            _iisServer.Stop();
        }
    }
}
