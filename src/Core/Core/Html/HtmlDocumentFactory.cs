namespace AspUnitRunner.Core.Html {
    internal class HtmlDocumentFactory : IHtmlDocumentFactory {
        public IHtmlDocument Create(string html) {
            return new HtmlDocument(html);
        }
    }
}
