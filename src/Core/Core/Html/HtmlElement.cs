using System;
using System.Collections.Generic;

namespace AspUnitRunner.Core.Html {
    internal class HtmlElement : IHtmlElement {
        private IDictionary<string, string> _attributes;

        public HtmlElement() {
            TagName = "";
            _attributes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            InnerHtml = "";
        }

        public string TagName { get; set; }

        public string ClassName {
            get {
                if (_attributes.ContainsKey("class"))
                    return _attributes["class"];
                return "";
            }
        }

        public IDictionary<string, string> Attributes {
            get { return _attributes; }
            set {
                _attributes = new Dictionary<string, string>(value,
                    StringComparer.InvariantCultureIgnoreCase);
            }
        }

        public string InnerHtml { get; set; }

        // note that this does not strip html tags - it's intended for leaf elements
        public string Text {
            get { return InnerHtml.Trim(); }
        }

        public IHtmlCollection GetElementsByTagName(string tagName) {
            return HtmlElementParser.GetElementsByTagName(InnerHtml, tagName);
        }
    }
}
