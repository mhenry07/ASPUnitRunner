using NUnit.Framework;
using AspUnitRunner;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.IntegrationTests {
    [TestFixture]
    public class TestRunner {
        private const string AddressField = "_address";

        [Test]
        public void Create_should_return_new_runner_with_address() {
            const string address = "http://path/to/test-runner";
            var runner = Runner.Create(address);

            Assert.That(runner, Is.InstanceOf<Runner>());
            Assert.That(runner.GetField(AddressField), Is.EqualTo(address));
        }
    }
}
