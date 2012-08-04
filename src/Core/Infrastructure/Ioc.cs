using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    // a very simple inversion of control container to resolve dependencies
    internal class Ioc {
        public static Runner ResolveRunner() {
            return new Runner(
                new AspClient(
                    new WebClientFactory(),
                    new ResponseDecoder()));
        }
    }
}
