namespace AspUnitRunner.Core.Html {
    internal class HtmlElement : IHtmlElement {
        public HtmlElement() {
            TagName = "";
            Attributes = "";
            InnerHtml = "";
        }

        public string TagName { get; set; }

        public string Attributes { get; set; }

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
