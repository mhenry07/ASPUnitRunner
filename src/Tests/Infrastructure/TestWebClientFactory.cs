using System.Net;
using NUnit.Framework;
using AspUnitRunner.Infrastructure;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Infrastructure {
    [TestFixture]
    public class TestWebClientFactory {
        [Test]
        public void Create_should_return_WebClient() {
            var factory = new WebClientFactory();
            using (var client = factory.Create()) {
                Assert.That(client, Is.InstanceOf<WebClient>());
            }
        }
    }
}
