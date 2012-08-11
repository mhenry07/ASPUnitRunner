using System.Collections.Generic;

namespace AspUnitRunner.Core.Html {
    internal interface IHtmlElementCollection : IEnumerable<IHtmlElement> {
        int Count { get; }
        IHtmlElement First { get; }
        IHtmlElement this[int index] { get; }
    }
}
