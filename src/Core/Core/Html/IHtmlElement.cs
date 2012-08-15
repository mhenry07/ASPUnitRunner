using System.Collections.Generic;

namespace AspUnitRunner.Core.Html {
    // note that this interface only exposes read-only properties and methods
    internal interface IHtmlElement {
        string TagName { get; }
        string ClassName { get; }
        IList<KeyValuePair<string, string>> Attributes { get; }
        string InnerHtml { get; }
        string Text { get; }
        string GetAttribute(string name);
        IHtmlCollection GetElementsByTagName(string tagName);
    }
}
