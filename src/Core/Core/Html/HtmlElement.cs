using System.Text.RegularExpressions;

namespace AspUnitRunner.Core.Html {
    internal class HtmlElement {
        protected readonly Match _match;

        public HtmlElement(Match match) {
            _match = match;
        }

        public string Attributes {
            get { return _match.Groups["attribs"].Value; }
        }

        public string InnerHtml {
            get { return _match.Groups["innerHtml"].Value; }
         }

        public string Text {
            get { return InnerHtml.Trim(); }
        }

        public HtmlElementCollection GetDescendants(string tagName) {
            return HtmlElementCollection.GetElements(InnerHtml, tagName);
        }
    }
}
