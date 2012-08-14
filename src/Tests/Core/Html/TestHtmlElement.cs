using NUnit.Framework;
using AspUnitRunner.Core.Html;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Core.Html {
    [TestFixture]
    public class TestHtmlElement {
        [Test]
        public void Default_element_should_have_empty_properties() {
            var expectedElement = new HtmlElement {
                TagName = "",
                Attributes = "",
                InnerHtml = ""
            };

            Assert.That(new HtmlElement(), Is.EqualTo(expectedElement)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void Text_for_element_with_whitespace_should_be_empty() {
            var element = new HtmlElement {
                InnerHtml = " "
            };
            Assert.That(element.Text, Is.Empty);
        }

        [Test]
        public void Text_for_element_with_text_should_be_trimmed_text() {
            var element = new HtmlElement {
                InnerHtml = " text "
            };
            Assert.That(element.Text, Is.EqualTo("text"));
        }

        [Test]
        public void GetElementsByTagName_for_empty_paragraph_should_return_empty_collection() {
            var element = new HtmlElement {
                TagName = "p",
                InnerHtml = ""
            };
            Assert.That(element.GetElementsByTagName("p"), Is.Empty);
        }

        [Test]
        public void GetElementsByTagName_for_paragraph_with_inner_span_should_return_expected_collection() {
            var expectedElements = new[] {
                new HtmlElement { TagName = "span", Attributes = " class=\"class\"", InnerHtml = "text" }
            };
            var element = new HtmlElement {
                TagName = "p",
                InnerHtml = "<span class=\"class\">text</span>"
            };

            var elements = element.GetElementsByTagName("span");

            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }
    }
}
