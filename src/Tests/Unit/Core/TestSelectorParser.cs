using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner.Core;
using AspUnitRunner.Core.Html;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Unit.Core {
    [TestFixture]
    public class TestSelectorParser {
        [Test]
        public void ParseTestContainers_empty_should_return_empty_list() {
            var html = FakeTestFormatter.FormatSelector(new string[] { }, null);

            var parser = CreateSelectorParser(html);
            var containers = parser.ParseContainers(html);
            Assert.That(containers, Is.Empty);
        }

        [Test]
        public void ParseTestContainers_should_return_expected_containers() {
            var expectedContainers = new[] {
                "first",
                "second"
            };
            var html = FakeTestFormatter.FormatSelector(expectedContainers, null);

            var parser = CreateSelectorParser(html);
            var containers = parser.ParseContainers(html);
            Assert.That(containers, Is.EqualTo(expectedContainers));
        }

        private SelectorParser CreateSelectorParser(string html) {
            var htmlDocumentFactory = MockRepository.GenerateStub<IHtmlDocumentFactory>();
            htmlDocumentFactory.Stub(f => f.Create(html))
                .Return(new HtmlDocument(html));
            return new SelectorParser(htmlDocumentFactory);
        }
    }
}
