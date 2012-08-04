using NUnit.Framework;
using AspUnitRunner;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestResults {
        [Test]
        public void Constructor_should_set_properties_to_expected_values() {
            const string html = "<html><body></body></html>";
            var results = new Results(3, 2, 1, html);

            Assert.That(results.Tests, Is.EqualTo(3));
            Assert.That(results.Errors, Is.EqualTo(2));
            Assert.That(results.Failures, Is.EqualTo(1));
            Assert.That(results.Html, Is.EqualTo(html));
        }
    }
}
