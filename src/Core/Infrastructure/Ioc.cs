using AspUnitRunner.Core;
using AspUnitRunner.Core.Html;

namespace AspUnitRunner.Infrastructure {
    // a very simple inversion of control container to resolve dependencies
    internal class Ioc {
        public static IRunner ResolveRunner() {
            var htmlDocumentFactory = new HtmlDocumentFactory();

            return new AspRunner(
                new AspClient(
                    new WebClientFactory(),
                    new ResponseDecoder()),
                new ResultParser(htmlDocumentFactory),
                new SelectorParser(htmlDocumentFactory));
        }
    }
}
