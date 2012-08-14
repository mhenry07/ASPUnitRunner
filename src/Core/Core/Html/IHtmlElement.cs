namespace AspUnitRunner.Core.Html {
    internal interface IHtmlElement {
        string TagName { get; }
        string Attributes { get; }
        string InnerHtml { get; }
        string Text { get; }
        IHtmlCollection GetElementsByTagName(string tagName);
    }
}
