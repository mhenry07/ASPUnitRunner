using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AspUnitRunner.Core.Html {
    internal class HtmlElementCollection : IHtmlElementCollection {
        private const string HtmlElementRegex = @"<{0}\b(?<attribs>[^>]*)>(?<innerHtml>(?:.(?!<{0}\b))*)</{0}>";

        private readonly IList<IHtmlElement> _elements;

        public int Count {
            get { return _elements.Count; }
        }

        public IHtmlElement First {
            get {
                if (_elements.Count == 0)
                    return new HtmlElement(Match.Empty);
                return _elements[0];
            }
        }

        public IHtmlElement this[int index] {
            get { return _elements[index]; }
        }

        public static IHtmlElementCollection GetElements(string html, string tagName) {
            var elementMatches = GetElementMatches(html, tagName);
            return new HtmlElementCollection(elementMatches);
        }

        private static MatchCollection GetElementMatches(string html, string tagName) {
            var elementRegex = GetElementRegex(tagName);
            return elementRegex.Matches(html);
        }

        private static Regex GetElementRegex(string tagName) {
            var elementPattern = string.Format(HtmlElementRegex, tagName);
            return new Regex(elementPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        private HtmlElementCollection(MatchCollection matches) {
            _elements = new List<IHtmlElement>();
            foreach (Match match in matches)
                _elements.Add(new HtmlElement(match));
        }

        public IEnumerator<IHtmlElement> GetEnumerator() {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
