using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public class Runner {
        /// <summary>
        /// Creates a new AspUnitRunner.IRunner instance with the specified address.
        /// </summary>
        /// <param name="address">The URL for the ASPUnit tests.</param>
        /// <returns>A new AspUnitRunner.IRunner instance.</returns>
        public static IRunner Create(string address) {
            var runner = (AspRunner)Infrastructure.Ioc.ResolveRunner();
            return runner.WithAddress(address);
        }
    }
}
