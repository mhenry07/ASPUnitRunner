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
                InnerHtml = ""
            };

            Assert.That(new HtmlElement(), Is.EqualTo(expectedElement)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void ClassName_without_class_should_be_null() {
            var element = new HtmlElement();
            Assert.That(element.ClassName, Is.Null);
        }

        [Test]
        public void ClassName_with_class_should_get_expected_value() {
            var element = new HtmlElement();
            element.SetAttribute("class", "a");
            Assert.That(element.ClassName, Is.EqualTo("a"));
        }

        [Test]
        public void ClassName_with_uppercase_class_should_get_expected_value() {
            var element = new HtmlElement();
            element.SetAttribute("CLASS", "a");
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
        public void Text_for_element_with_text_should_get_trimmed_text() {
            var element = new HtmlElement {
                InnerHtml = " text "
            };
            Assert.That(element.Text, Is.EqualTo("text"));
        }

        [Test]
        public void GetAttribute_not_found_should_return_null() {
            var element = new HtmlElement();
            Assert.That(element.GetAttribute("class"), Is.Null);
        }

        [Test]
        public void GetAttribute_should_return_expected_value() {
            var element = new HtmlElement();
            element.SetAttribute("class", "a");
            Assert.That(element.GetAttribute("class"), Is.EqualTo("a"));
        }

        [Test]
        public void GetAttribute_uppercase_should_return_expected_value() {
            var element = new HtmlElement();
            element.SetAttribute("class", "a");
            Assert.That(element.GetAttribute("CLASS"), Is.EqualTo("a"));
        }

        [Test]
        public void SetAttribute_should_add_attribute() {
            var expectedAttributes = new AttributeTestList { { "name", "value" } };

            var element = new HtmlElement();
            element.SetAttribute("name", "value");
            Assert.That(element.Attributes, Is.EqualTo(expectedAttributes));
        }

        [Test]
        public void SetAttribute_should_add_attribute_with_lowercase_name() {
            var expectedAttributes = new AttributeTestList { { "name", "value" } };

            var element = new HtmlElement();
            element.SetAttribute("NAME", "value");
            Assert.That(element.Attributes, Is.EqualTo(expectedAttributes));
        }

        [Test]
        public void SetAttribute_should_update_existing_attribute() {
            var expectedAttributes = new AttributeTestList { { "name", "second" } };

            var element = new HtmlElement();
            element.SetAttribute("name", "first");
            element.SetAttribute("name", "second");
            Assert.That(element.Attributes, Is.EqualTo(expectedAttributes));
        }

        [Test]
        public void SetAttribute_should_update_existing_attribute_with_lowercase_name() {
            var expectedAttributes = new AttributeTestList { { "name", "second" } };

            var element = new HtmlElement();
            element.SetAttribute("name", "first");
            element.SetAttribute("NAME", "second");
            Assert.That(element.Attributes, Is.EqualTo(expectedAttributes));
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
                new HtmlElement { TagName = "span", InnerHtml = "text" }
            };
            expectedElements[0].SetAttribute("class", "class");
            var element = new HtmlElement {
                TagName = "p",
                InnerHtml = "<span class=\"class\">text</span>"
            };

            var elements = element.GetElementsByTagName("span");

            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        private class AttributeTestList : List<KeyValuePair<string, string>> {
            public void Add(string key, string value) {
                Add(new KeyValuePair<string, string>(key, value));
            }
        }
    }
}
