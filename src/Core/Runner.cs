using System;
using System.Net;
using AspUnitRunner.Core;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public class Runner {
        private readonly IRunner _runner;

        /// <summary>
        /// Creates a new AspUnitRunner.IRunner instance with the specified address.
        /// </summary>
        /// <param name="address">The URL for the ASPUnit tests.</param>
        /// <returns>A new AspUnitRunner.IRunner instance.</returns>
        public static IRunner Create(string address) {
            var runner = (AspRunner)Infrastructure.Ioc.ResolveRunner();
            return runner.WithAddress(address);
        }

        /// <summary>
        /// Use Runner.Create() instead.
        /// </summary>
        /// <param name="baseUri">The URL for the ASPUnit tests.</param>
        [Obsolete]
        public Runner(string baseUri)
            : this(baseUri, null) {
        }

        /// <summary>
        /// Use Runner.Create().WithCredentials() instead.
        /// </summary>
        /// <param name="baseUri">The URL for the ASPUnit tests.</param>
        /// <param name="credentials">The network credentials.</param>
        [Obsolete]
        public Runner(string baseUri, ICredentials credentials) {
            _runner = Runner.Create(baseUri)
                .WithCredentials(credentials);
        }

        /// <summary>
        /// Use IRunner.WithTestContainer() and IRunner.Run() instead.
        /// </summary>
        /// <param name="testContainer">The test container.</param>
        /// <returns>The test results.</returns>
        [Obsolete]
        public Results Run(string testContainer) {
            return (Results)_runner
                .WithTestContainer(testContainer)
                .Run();
        }
    }
}
