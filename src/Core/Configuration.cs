using System.Net;

namespace AspUnitRunner {
    /// <summary>
    /// Stores configuration values for Runner.
    /// </summary>
    public class Configuration {
        /// <summary>
        /// Gets or sets the network credentials used to authenticate the request.
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>
        /// Gets or sets the name of the test container from which to run tests.
        /// </summary>
        public string TestContainer { get; set; }

        /// <summary>
        /// Gets or sets the name of the test case to execute.
        /// </summary>
        public string TestCase { get; set; }
    }
}
