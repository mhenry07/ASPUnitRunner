namespace AspUnitRunner.Core.Html {
    internal class HtmlDocFactory : IHtmlDocFactory {
        public IHtmlDoc Create(string html) {
            return new HtmlDoc(html);
        }
    }
}
