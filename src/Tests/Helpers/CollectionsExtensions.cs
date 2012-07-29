using System.Collections.Specialized;
using System.Linq;

namespace AspUnitRunner.Tests.Helpers {
    public static class CollectionsExtensions {
        public static bool SequenceEqual(this NameValueCollection first, NameValueCollection second) {
            return first.AllKeys.SequenceEqual(second.AllKeys)
                && first.AllKeys.All(key => first.GetValues(key).SequenceEqual(second.GetValues(key)));
        }
    }
}
