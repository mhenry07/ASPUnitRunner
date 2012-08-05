using System.Collections.Generic;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Contains ASPUnit test results.
    /// </summary>
    public class Results {
        /// <summary>
        /// Gets the number of tests run.
        /// </summary>
        public int Tests { get; internal set; }

        /// <summary>
        /// Gets the number of test errors.
        /// </summary>
        public int Errors { get; internal set; }

        /// <summary>
        /// Gets the number of test failures.
        /// </summary>
        public int Failures { get; internal set; }

        /// <summary>
        /// Gets the collection of test details.
        /// </summary>
        public IEnumerable<ResultDetail> Details { get; internal set; }

        /// <summary>
        /// Gets the raw HTML test results.
        /// </summary>
        public string Html { get; internal set; }

        internal Results() {
        }
    }
}
