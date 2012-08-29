using System.Collections.Generic;

namespace AspUnitRunner.Core {
    internal interface ISelectorParser {
        IEnumerable<string> ParseContainers(string html);
    }
}
