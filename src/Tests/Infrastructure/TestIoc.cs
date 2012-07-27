using NUnit.Framework;
using AspUnitRunner.Infrastructure;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Infrastructure {
    [TestFixture]
    public class TestIoc {
        [Test]
        public void ResolveAspProxy_should_return_expected_object_graph() {
            var proxy = Ioc.ResolveAspProxy();
            Assert.That(proxy, Is.InstanceOf<AspProxy>());

            Assert.That(proxy.GetField("_webRequestFactory"),
                Is.InstanceOf<WebRequestFactory>());
        }
    }
}
