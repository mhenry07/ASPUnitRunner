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

            var proxy = runner.GetField("_proxy");
            Assert.That(proxy, Is.InstanceOf<AspProxy>());

            Assert.That(proxy.GetField("_webRequestFactory"),
                Is.InstanceOf<WebRequestFactory>());
        }
    }
}
