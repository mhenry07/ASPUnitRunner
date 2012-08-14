using System;
using System.Collections.Generic;
using AspUnitRunner.Core.Html;

namespace AspUnitRunner.Tests.Helpers {
    internal class HtmlElementEqualityComparer : IEqualityComparer<HtmlElement> {
        public bool Equals(HtmlElement x, HtmlElement y) {
            return string.Compare(x.TagName, y.TagName, StringComparison.InvariantCultureIgnoreCase) == 0
                && x.Attributes.Equals(y.Attributes) && x.InnerHtml.Equals(y.InnerHtml);
        }

        public int GetHashCode(HtmlElement obj) {
            throw new NotImplementedException();
        }
    }
}
