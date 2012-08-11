namespace AspUnitRunner.Core.Html {
    internal class HtmlDoc : IHtmlDoc {
        private readonly string _html;

        public HtmlDoc(string html) {
            _html = html;
        }

        public IHtmlElementCollection GetDescendants(string tagName) {
            return HtmlElementCollection.GetElements(_html, tagName);
        }
    }
}
