using System.Collections.Generic;

namespace AspUnitRunner.Core.Html {
    internal interface IHtmlElement {
        string TagName { get; }
        string ClassName { get; }
        IDictionary<string, string> Attributes { get; }
        string InnerHtml { get; }
        string Text { get; }
        IHtmlCollection GetElementsByTagName(string tagName);
    }
}
