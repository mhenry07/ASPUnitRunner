using System;
using System.Collections.Generic;
using AspUnitRunner.Core.Html;

namespace AspUnitRunner.Core {
    internal class SelectorParser : ISelectorParser {
        private readonly IHtmlDocumentFactory _htmlDocumentFactory;

        public SelectorParser(IHtmlDocumentFactory htmlDocumentFactory) {
            _htmlDocumentFactory = htmlDocumentFactory;
        }

        public IEnumerable<string> ParseContainers(string html) {
            throw new NotImplementedException();
        }

        public IEnumerable<string> ParseTestCases(string html, string container) {
            throw new NotImplementedException();
        }
    }
}
