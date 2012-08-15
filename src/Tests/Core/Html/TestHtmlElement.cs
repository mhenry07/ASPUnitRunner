using System.Collections.Generic;
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
                Attributes = new Dictionary<string, string>(),
                InnerHtml = ""
            };

            Assert.That(new HtmlElement(), Is.EqualTo(expectedElement)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void ClassName_without_class_should_get_empty_string() {
            var element = new HtmlElement();
            Assert.That(element.ClassName, Is.Empty);
        }

        [Test]
        public void ClassName_with_class_should_get_expected_value() {
            var element = new HtmlElement {
                Attributes = new Dictionary<string, string> { { "class", "a" } }
            };
            Assert.That(element.ClassName, Is.EqualTo("a"));
        }

        [Test]
        public void ClassName_with_upper_case_class_should_get_expected_value() {
            var element = new HtmlElement {
                Attributes = new Dictionary<string, string> { { "CLASS", "a" } }
            };
            Assert.That(element.ClassName, Is.EqualTo("a"));
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
            var attributes = new Dictionary<string, string> { { "class", "class" } };
            var expectedElements = new[] {
                new HtmlElement { TagName = "span", Attributes = attributes, InnerHtml = "text" }
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
