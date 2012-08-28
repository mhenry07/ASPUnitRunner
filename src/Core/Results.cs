using System;
using System.Collections.Generic;
using System.Text;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    // TODO: move to Core and make internal after old api has been removed
    /// <summary>
    /// Contains ASPUnit test results.
    /// </summary>
    public class Results : IResults {
        internal Results() {
        }

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
        /// Gets a bool indicating whether all tests ran successfully.
        /// </summary>
        public bool Successful {
            get {
                return Errors == 0 && Failures == 0;
            }
        }

        /// <summary>
        /// Gets the collection of test details.
        /// </summary>
        public IEnumerable<ResultDetail> Details { get; internal set; }

        /// <summary>
        /// Gets the raw HTML test results.
        /// </summary>
        /// <remarks>
        /// May contain a long HTML string which NUnit doesn't format very well.
        /// </remarks>
        public string Html { get; internal set; }

        /// <summary>
        /// Returns a string containing formatted test results.
        /// </summary>
        /// <returns>A string containing formatted test results.</returns>
        public string Format() {
            return string.Format("{0}{2}{2}{1}{2}",
                FormatDetails(), FormatSummary(), Environment.NewLine);
        }

        internal string FormatDetails() {
            var newline = "";
            var stringBuilder = new StringBuilder();
            foreach (var detail in Details) {
                stringBuilder.Append(newline);
                stringBuilder.AppendFormat("{0}: {1}: {2}",
                    detail.Type, detail.Name, detail.Description);
                newline = Environment.NewLine;
            }

            return stringBuilder.ToString();
        }

        internal string FormatSummary() {
            return string.Format("Tests: {0}, Errors: {1}, Failures: {2}",
                Tests, Errors, Failures);
        }
    }
}
