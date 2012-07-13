using NUnit.Framework;
using AspUnitRunner;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestResults {
        [Test]
        public void Constructor_should_set_properties_to_expected_values() {
            var results = new Results(1, 2, 3, "details");

            Assert.That(results.Tests, Is.EqualTo(1));
            Assert.That(results.Errors, Is.EqualTo(2));
            Assert.That(results.Failures, Is.EqualTo(3));
            Assert.That(results.Details, Is.EqualTo("details"));
        }
    }
}
