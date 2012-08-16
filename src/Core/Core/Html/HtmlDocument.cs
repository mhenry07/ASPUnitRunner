namespace AspUnitRunner.Core.Html {
    internal class HtmlDocument : IHtmlDocument {
        private readonly string _html;

        public HtmlDocument(string html) {
            _html = html;
        }

        public IHtmlCollection GetElementsByTagName(string tagName) {
            return HtmlElementParser.GetElementsByTagName(_html, tagName);
        }
    }
}
