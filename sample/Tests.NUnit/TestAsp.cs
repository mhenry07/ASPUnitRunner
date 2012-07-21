using NUnit.Framework;
using AspUnitRunner;

namespace AspUnitRunner.Sample.Tests.NUnit {
    [TestFixture]
    public class TestAsp {
        // set the URL for your ASPUnit tests
        private const string AspTestUrl = "http://localhost:54831/tests/Default.asp";

        [Test]
        public void TestCase(
            // set ASPUnit test containers here
            [Values("CalculatorTest", "StringUtilityTest", "FailureTest")] string testContainer
            ) {
            var runner = new Runner(AspTestUrl);
            var results = runner.Run(testContainer);

            // Note: results.Details can generate a long HTML string which NUnit doesn't format very well
            Assert.That(results.Errors, Is.EqualTo(0), results.Details);
            Assert.That(results.Failures, Is.EqualTo(0), results.Details);
        }
    }
}
