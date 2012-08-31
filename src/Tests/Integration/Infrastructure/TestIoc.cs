using NUnit.Framework;
using AspUnitRunner.Core;
using AspUnitRunner.Core.Html;
using AspUnitRunner.Infrastructure;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Integration.Infrastructure {
    [TestFixture]
    public class TestIoc {
        [Test]
        public void ResolveRunner_should_return_expected_object_graph() {
            var runner = Ioc.ResolveRunner();
            Assert.That(runner, Is.InstanceOf<AspRunner>());

            var client = runner.GetField("_client");
            Assert.That(client, Is.InstanceOf<AspClient>());
            var resultParser = runner.GetField("_resultParser");
            Assert.That(resultParser, Is.InstanceOf<ResultParser>());
            var selectorParser = runner.GetField("_selectorParser");
            Assert.That(selectorParser, Is.InstanceOf<SelectorParser>());

            Assert.That(client.GetField("_factory"),
                Is.InstanceOf<WebClientFactory>());
            Assert.That(client.GetField("_responseDecoder"),
                Is.InstanceOf<ResponseDecoder>());

            Assert.That(resultParser.GetField("_htmlDocumentFactory"),
                Is.InstanceOf<HtmlDocumentFactory>());
            Assert.That(selectorParser.GetField("_htmlDocumentFactory"),
                Is.InstanceOf<HtmlDocumentFactory>());
        }
    }
}
