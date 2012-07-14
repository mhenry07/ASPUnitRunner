using NUnit.Framework;
using AspUnitRunner;

namespace asp.NUnitTests {
    [TestFixture]
    public class TestAsp {
        // set the URL for your ASPUnit tests
        private const string AspUnitUrl = "http://localhost/AspUnitRunner/asp.tests/";

        [Test]
        public void TestCase(
            // set ASPUnit test containers here
            [Values("CalculatorTest", "StringUtilityTest")] string testContainer
            ) {
            var runner = new Runner(AspUnitUrl);
            var results = runner.Run(testContainer);

            // Note: results.Details can generate a long HTML string which NUnit doesn't format very well
            Assert.That(results.Errors, Is.EqualTo(0), results.Details);
            Assert.That(results.Failures, Is.EqualTo(0), results.Details);
        }
    }
}
