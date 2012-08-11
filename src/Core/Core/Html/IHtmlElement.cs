using System.Text.RegularExpressions;

namespace AspUnitRunner.Core.Html {
    internal interface IHtmlElement {
        string Attributes { get; }
        string InnerHtml { get; }
        string Text { get; }
        IHtmlElementCollection GetDescendants(string tagName);
    }
}
