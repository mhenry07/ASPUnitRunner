using System.Collections.Generic;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Contains ASPUnit test results.
    /// </summary>
    public interface IResults {
        /// <summary>
        /// Gets the number of tests run.
        /// </summary>
        int Tests { get; }

        /// <summary>
        /// Gets the number of test errors.
        /// </summary>
        int Errors { get; }

        /// <summary>
        /// Gets the number of test failures.
        /// </summary>
        int Failures { get; }

        /// <summary>
        /// Gets a bool indicating whether all tests ran successfully.
        /// </summary>
        bool Successful { get; }

        /// <summary>
        /// Gets the collection of test details.
        /// </summary>
        IEnumerable<ResultDetail> Details { get; }

        /// <summary>
        /// Gets the raw HTML test results.
        /// </summary>
        /// <remarks>
        /// May contain a long HTML string which NUnit doesn't format very well.
        /// </remarks>
        string Html { get; }

        /// <summary>
        /// Returns a string containing formatted test results.
        /// </summary>
        /// <returns>A string containing formatted test results.</returns>
        string Format();
    }
}
