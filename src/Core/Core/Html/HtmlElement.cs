using System.Collections.Generic;

namespace AspUnitRunner.Core.Html {
    internal class HtmlElement : IHtmlElement {
        // Using a list rather than a dictionary to make it easier to provide 
        // a read-only Attributes getter. Attribute lists should be small so 
        // performance impact should be negligible.
        private readonly NameValueList _attributes = new NameValueList();

        public HtmlElement() {
            TagName = "";
            InnerHtml = "";
        }

        public string TagName { get; set; }

        public string ClassName {
            get { return GetAttribute("class"); }
        }

        public IList<KeyValuePair<string, string>> Attributes {
            get { return _attributes.AsReadOnly(); }
        }

        public string InnerHtml { get; set; }

        // note that this does not strip html tags - it's intended for leaf elements
        public string Text {
            get { return InnerHtml.Trim(); }
        }

        public string GetAttribute(string name) {
            return _attributes[name];
        }

        public void SetAttribute(string name, string value) {
            var lowerCaseName = name.ToLowerInvariant();
            _attributes[lowerCaseName] = value;
        }

        public IHtmlCollection GetElementsByTagName(string tagName) {
            return HtmlElementParser.GetElementsByTagName(InnerHtml, tagName);
        }
    }
}
