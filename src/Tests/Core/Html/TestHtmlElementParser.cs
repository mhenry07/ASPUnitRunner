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
                new HtmlElement { TagName = "p", Attributes = " class=\"class\"", InnerHtml = "text" }
            };
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
            const string attributes = @" BORDER=""1"" WIDTH=""80%""";
            const string innerHtml =
@"
		<TR CLASS=""warning""><TD>Failure</TD><TD>Container.TestCase</TD><TD>Description</TD></TR>
	";
            var expectedElements = new[] {
                new HtmlElement { TagName = "TABLE", Attributes = attributes, InnerHtml = innerHtml }
            };

            const string htmlFormat =
@"<HTML>
<BODY>
	<TABLE{0}>{1}</TABLE>
</BODY>
</HTML>";
            var html = string.Format(htmlFormat, attributes, innerHtml);

            var elements = HtmlElementParser.GetElementsByTagName(html, "TABLE");
            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetElementsByTagName_for_table_should_return_expected_row() {
            const string attributes = @" CLASS=""warning""";
            const string innerHtml =
                @"<TD>Failure</TD><TD>Container.TestCase</TD><TD>Description</TD>";
            const string htmlFormat =
@"	<TABLE>
		<TR{0}>{1}</TR>
	</TABLE>";
            var html = string.Format(htmlFormat, attributes, innerHtml);
            var expectedElements = new[] {
                new HtmlElement { TagName = "TR", Attributes = attributes, InnerHtml = innerHtml }
            };

            var elements = HtmlElementParser.GetElementsByTagName(html, "TR");
            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }

        [Test]
        public void GetElementsByTagName_for_row_should_return_expected_cells() {
            var innerHtml = new[] { "Failure", "Container.TestCase", "Description" };
            var expectedElements = new[] {
                new HtmlElement { TagName = "TD", InnerHtml = innerHtml[0] },
                new HtmlElement { TagName = "TD", InnerHtml = innerHtml[1] },
                new HtmlElement { TagName = "TD", InnerHtml = innerHtml[2] }
            };
            const string htmlFormat =
                @"<TR CLASS=""warning""><TD>{0}</TD><TD>{1}</TD><TD>{2}</TD></TR>";
            var html = string.Format(htmlFormat, innerHtml[0], innerHtml[1], innerHtml[2]);

            var elements = HtmlElementParser.GetElementsByTagName(html, "TD");
            Assert.That(elements, Is.EqualTo(expectedElements)
                .Using(new HtmlElementEqualityComparer()));
        }
    }
}
