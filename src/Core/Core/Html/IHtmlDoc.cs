namespace AspUnitRunner.Core.Html {
    internal interface IHtmlDoc {
        IHtmlElementCollection GetDescendants(string tagName);
    }
}
