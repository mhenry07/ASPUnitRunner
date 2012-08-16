namespace AspUnitRunner.Core.Html {
    internal interface IHtmlDocument {
        IHtmlCollection GetElementsByTagName(string tagName);
    }
}
