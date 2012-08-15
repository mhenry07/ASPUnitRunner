using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AspUnitRunner.Core.Html {
    internal class HtmlElement : IHtmlElement {
        // Using a list rather than a dictionary to make it easier to provide 
        // a read-only Attributes getter. Attribute lists should be small so 
        // performance impact should be negligible.
        private readonly IList<KeyValuePair<string, string>> _attributes =
            new List<KeyValuePair<string, string>>();

        public HtmlElement() {
            TagName = "";
            InnerHtml = "";
        }

        public string TagName { get; set; }

        public string ClassName {
            get { return GetAttribute("class"); }
        }

        public IList<KeyValuePair<string, string>> Attributes {
            get {
                return new ReadOnlyCollection<KeyValuePair<string, string>>(_attributes);
            }
        }

        public string InnerHtml { get; set; }

        // note that this does not strip html tags - it's intended for leaf elements
        public string Text {
            get { return InnerHtml.Trim(); }
        }

        public string GetAttribute(string name) {
            var key = name.ToLowerInvariant();
            foreach (var attribute in _attributes)
                if (attribute.Key == key)
                    return attribute.Value;
            return null;
        }

        public void SetAttribute(string name, string value) {
            var key = name.ToLowerInvariant();
            var newAttribute = new KeyValuePair<string, string>(key, value);
            for (var i = 0; i < _attributes.Count; i++)
                if (_attributes[i].Key == key) {
                    _attributes[i] = newAttribute;
                    return;
                }
            _attributes.Add(newAttribute);
        }

        public IHtmlCollection GetElementsByTagName(string tagName) {
            return HtmlElementParser.GetElementsByTagName(InnerHtml, tagName);
        }
    }
}
