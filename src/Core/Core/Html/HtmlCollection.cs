using System.Collections;
using System.Collections.Generic;

namespace AspUnitRunner.Core.Html {
    internal class HtmlCollection : IHtmlCollection {
        private readonly IList<IHtmlElement> _elements;

        public HtmlCollection(IList<IHtmlElement> elements) {
            _elements = elements;
        }

        public int Length {
            get { return _elements.Count; }
        }

        public IHtmlElement First {
            get {
                if (Length == 0)
                    return new HtmlElement();
                return this[0];
            }
        }

        public IHtmlElement this[int index] {
            get { return _elements[index]; }
        }

        public IEnumerator<IHtmlElement> GetEnumerator() {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
