using System.Collections.Generic;
using NUnit.Framework;
using AspUnitRunner.Core.Html;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Core.Html {
    [TestFixture]
    public class TestHtmlElementParser {
        [Test]
        public void GetElementsByTagName_should_return_empty_collection() {
            var elements = HtmlElementParser.GetElementsByTagName("", "p");

            Assert.That(elements, Is.Empty);
        }

        [Test]
        public void GetElementsByTagName_for_whitespace_should_return_empty_collection() {
            var elements = HtmlElementParser.GetElementsByTagName(" ", "p");
            Assert.That(elements, Is.Empty);
        }

        [Test]
        public void GetElementsByTagName_text_should_return_empty_collection() {
            var elements = HtmlElementParser.GetElementsByTagName("text", "p");
            Assert.That(elements, Is.Empty);
        }

        [Test]
        public void GetElementsByTagName_for_p_should_return_expected_element() {
            var expectedElements = new[] {
                new HtmlElement { TagName = "p" }
            };
            var elements = HtmlElementParser.GetElementsByTagName("<p></p>", "p");

            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetElementsByTagName_for_upper_case_p_should_return_expected_element() {
            var expectedElements = new[] {
                new HtmlElement { TagName = "P" }
            };
            var elements = HtmlElementParser.GetElementsByTagName("<P></P>", "p");

            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetElementsByTagName_for_p_with_attributes_and_text_should_return_expected_element() {
            var expectedElements = new[] {
                new HtmlElement { TagName = "p", InnerHtml = "text" }
            };
            expectedElements[0].SetAttribute("class", "class");
            var elements = HtmlElementParser.GetElementsByTagName("<p class=\"class\">text</p>", "p");

            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetElementsByTagName_for_two_p_s_should_return_expected_elements() {
            var expectedElements = new[] {
                new HtmlElement { TagName = "p", InnerHtml = "first" },
                new HtmlElement { TagName = "p", InnerHtml = "second" }
            };
            var elements = HtmlElementParser.GetElementsByTagName("<p>first</p><p>second</p>", "p");

            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetElementsByTagName_for_document_should_return_expected_table() {
            const string innerHtml =
@"
		<TR CLASS=""warning""><TD>Failure</TD><TD>Container.TestCase</TD><TD>Description</TD></TR>
	";
            const string htmlFormat =
@"<HTML>
<BODY>
	<TABLE BORDER=""1"" WIDTH=""80%"">{0}</TABLE>
</BODY>
</HTML>";
            var html = string.Format(htmlFormat, innerHtml);
            var expectedElements = new[] {
                new HtmlElement { TagName = "TABLE", InnerHtml = innerHtml }
            };
            expectedElements[0].SetAttribute("BORDER", "1");
            expectedElements[0].SetAttribute("WIDTH", "80%");

            var elements = HtmlElementParser.GetElementsByTagName(html, "TABLE");
            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetElementsByTagName_for_table_should_return_expected_row() {
            const string innerHtml =
                @"<TD>Failure</TD><TD>Container.TestCase</TD><TD>Description</TD>";
            const string htmlFormat =
@"	<TABLE>
		<TR CLASS=""warning"">{0}</TR>
	</TABLE>";
            var html = string.Format(htmlFormat, innerHtml);
            var expectedElements = new[] {
                new HtmlElement { TagName = "TR", InnerHtml = innerHtml }
            };
            expectedElements[0].SetAttribute("CLASS", "warning");

            var elements = HtmlElementParser.GetElementsByTagName(html, "TR");
            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetElementsByTagName_for_row_should_return_expected_cells() {
            const string html =
                @"<TR CLASS=""warning""><TD>Failure</TD><TD>Container.TestCase</TD><TD>Description</TD></TR>";
            var expectedElements = new[] {
                new HtmlElement { TagName = "TD", InnerHtml = "Failure" },
                new HtmlElement { TagName = "TD", InnerHtml = "Container.TestCase" },
                new HtmlElement { TagName = "TD", InnerHtml = "Description" }
            };

            var elements = HtmlElementParser.GetElementsByTagName(html, "TD");
            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }
    }
}
