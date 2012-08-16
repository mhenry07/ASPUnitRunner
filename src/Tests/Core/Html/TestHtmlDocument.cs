using NUnit.Framework;
using AspUnitRunner.Core.Html;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Core.Html {
    [TestFixture]
    public class TestHtmlDocument {
        [Test]
        public void GetElementsByTagName_should_get_expected_elements() {
            var expectedElements = new[] {
                new HtmlElement { TagName = "table", InnerHtml = "<tr><td></td></tr>" }
            };
            const string html = "<html><body><table><tr><td></td></tr></table></body></html>";

            var doc = new HtmlDocument(html);
            var elements = doc.GetElementsByTagName("table");
            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }
    }
}
