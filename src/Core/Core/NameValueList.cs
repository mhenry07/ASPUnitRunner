using System;
using System.Collections.Generic;

namespace AspUnitRunner.Core {
    // note that name (key) comparisons are case-insensitive
    internal class NameValueList : List<KeyValuePair<string, string>> {
        private readonly StringComparer _comparer =
            StringComparer.OrdinalIgnoreCase;

        // get returns null if not found
        public string this[string name] {
            get {
                var index = IndexOf(name);
                if (index == -1)
                    return null;
                return this[index].Value;
            }
            set {
                var index = IndexOf(name);
                if (index == -1) {
                    Add(new KeyValuePair<string, string>(name, value));
                    return;
                }
                this[index] = new KeyValuePair<string, string>(this[index].Key, value);
            }
        }

        private int IndexOf(string name) {
            for (var i = 0; i < Count; i++)
                if (NameEquals(this[i], name))
                    return i;
            return -1;
        }

        private bool NameEquals(KeyValuePair<string, string> item, string name) {
            return _comparer.Equals(item.Key, name);
        }
    }
}
