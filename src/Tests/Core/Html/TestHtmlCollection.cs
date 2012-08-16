using System.Collections.Generic;
using NUnit.Framework;
using AspUnitRunner.Core.Html;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Core.Html {
    [TestFixture]
    public class TestHtmlCollection {
        [Test]
        public void Count_of_empty_collection_should_be_0() {
            var elements = new IHtmlElement[] { };
            var collection = new HtmlCollection(elements);

            Assert.That(collection.Count, Is.EqualTo(0));
        }

        [Test]
        public void Count_of_collection_with_one_element_should_be_1() {
            var elements = new IHtmlElement[] {
                new HtmlElement()
            };
            var collection = new HtmlCollection(elements);

            Assert.That(collection.Count, Is.EqualTo(1));
        }

        [Test]
        public void First_should_get_first_element() {
            var elements = new IHtmlElement[] {
                new HtmlElement { TagName = "p", InnerHtml = "first" },
                new HtmlElement { TagName = "p", InnerHtml = "second" }
            };
            var collection = new HtmlCollection(elements);

            Assert.That(collection.First, Is.EqualTo(elements[0])
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void First_for_empty_collection_should_get_empty_element() {
            var expectedElement = new HtmlElement {
                TagName = "",
                InnerHtml = ""
            };
            var elements = new IHtmlElement[] { };
            var collection = new HtmlCollection(elements);

            Assert.That(collection.First, Is.EqualTo(new HtmlElement())
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void Indexer_0_should_get_expected_element() {
            var elements = new IHtmlElement[] {
                new HtmlElement { TagName = "p", InnerHtml = "first" },
                new HtmlElement { TagName = "p", InnerHtml = "second" }
            };
            var collection = new HtmlCollection(elements);

            Assert.That(collection[0], Is.EqualTo(elements[0])
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void Indexer_1_should_get_expected_element() {
            var elements = new IHtmlElement[] {
                new HtmlElement { TagName = "p", InnerHtml = "first" },
                new HtmlElement { TagName = "p", InnerHtml = "second" }
            };
            var collection = new HtmlCollection(elements);

            Assert.That(collection[1], Is.EqualTo(elements[1])
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetEnumerator_should_return_expected_enumerator() {
            var elements = new List<IHtmlElement> {
                new HtmlElement { TagName = "p", InnerHtml = "first" },
                new HtmlElement { TagName = "p", InnerHtml = "second" }
            };
            var collection = new HtmlCollection(elements);

            Assert.That(collection.GetEnumerator(), Is.EqualTo(elements.GetEnumerator()));
        }
    }
}
