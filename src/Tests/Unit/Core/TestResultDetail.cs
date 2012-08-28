using NUnit.Framework;
using AspUnitRunner.Core;

namespace AspUnitRunner.Tests.Unit.Core {
    [TestFixture]
    public class TestResultDetail {
        [Test]
        public void Constructor_should_set_properties_to_expected_values() {
            var detail = new ResultDetail(ResultType.Failure, "TestContainer.TestCase", "Error message");

            Assert.That(detail.Type, Is.EqualTo(ResultType.Failure));
            Assert.That(detail.Name, Is.EqualTo("TestContainer.TestCase"));
            Assert.That(detail.Description, Is.EqualTo("Error message"));
        }
    }
}
