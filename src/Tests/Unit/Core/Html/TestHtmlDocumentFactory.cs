using NUnit.Framework;
using AspUnitRunner.Core.Html;

namespace AspUnitRunner.Tests.Unit.Core.Html {
    [TestFixture]
    public class TestHtmlDocumentFactory {
        [Test]
        public void Create_should_return_HtmlDocument() {
            var factory = new HtmlDocumentFactory();
            var doc = factory.Create("<html></html>");
            Assert.That(doc, Is.InstanceOf<HtmlDocument>());
        }
    }
}
