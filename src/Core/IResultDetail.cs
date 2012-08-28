using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Contains the detail for a single ASPUnit test result.
    /// </summary>
    public interface IResultDetail {
        /// <summary>
        /// Gets the test result type/status.
        /// </summary>
        ResultType Type { get; }

        /// <summary>
        /// Gets the test name. (TestContainer.TestCase)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the test result description.
        /// </summary>
        string Description { get; }
    }
}
