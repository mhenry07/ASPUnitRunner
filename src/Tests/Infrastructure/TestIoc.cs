using NUnit.Framework;
using AspUnitRunner.Infrastructure;

namespace AspUnitRunner.Tests.Infrastructure {
    // note that this only tests the top-most dependency
    [TestFixture]
    public class TestIoc {
        [Test]
        public void ResolveAspProxy_should_return_AspProxy() {
            var proxy = Ioc.ResolveAspProxy();
            Assert.That(proxy, Is.TypeOf<AspProxy>());
        }
    }
}
