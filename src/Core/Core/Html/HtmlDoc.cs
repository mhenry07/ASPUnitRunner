namespace AspUnitRunner.Core.Html {
    internal class HtmlDoc {
        private readonly string _html;

        public HtmlDoc(string html) {
            _html = html;
        }

        public HtmlElementCollection GetDescendants(string tagName) {
            return HtmlElementCollection.GetElements(_html, tagName);
        }
    }
}
