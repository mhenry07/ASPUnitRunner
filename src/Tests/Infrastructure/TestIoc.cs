using NUnit.Framework;
using AspUnitRunner.Infrastructure;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Infrastructure {
    [TestFixture]
    public class TestIoc {
        [Test]
        public void ResolveRunner_should_return_expected_object_graph() {
            var runner = Ioc.ResolveRunner();
            Assert.That(runner, Is.InstanceOf<Runner>());

            var client = runner.GetField("_client");
            Assert.That(client, Is.InstanceOf<AspClient>());

            Assert.That(client.GetField("_factory"),
                Is.InstanceOf<WebClientFactory>());
            Assert.That(client.GetField("_responseDecoder"),
                Is.InstanceOf<ResponseDecoder>());
        }
    }
}
