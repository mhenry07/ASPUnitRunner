using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    // a very simple inversion of control container to resolve dependencies
    internal class Ioc {
        public static IAspProxy ResolveAspProxy() {
            return new AspProxy(new WebRequestFactory());
        }
    }
}
