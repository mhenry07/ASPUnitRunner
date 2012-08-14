using System.Collections.Generic;

namespace AspUnitRunner.Core.Html {
    internal interface IHtmlCollection : IEnumerable<IHtmlElement> {
        int Length { get; }
        IHtmlElement First { get; }
        IHtmlElement this[int index] { get; }
    }
}
