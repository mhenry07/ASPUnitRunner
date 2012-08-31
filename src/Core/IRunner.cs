using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public interface IRunner {
        /// <summary>
        /// Sets the network credentials used to authenticate the request
        /// and returns the current IRunner instance.
        /// </summary>
        /// <param name="credentials">The network credentials.</param>
        /// <returns>The current IRunner instance.</returns>
        IRunner WithCredentials(ICredentials credentials);

        /// <summary>
        /// Sets the default encoding used to encode the request and decode the
        /// response and returns the current IRunner instance.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The current IRunner instance.</returns>
        /// <remarks>A charset in the response headers will take precedence.</remarks>
        IRunner WithEncoding(Encoding encoding);

        /// <summary>
        /// Deprecated: Use Run(testContainer) instead.
        /// Sets the name of the test container from which to run tests
        /// and returns the current IRunner instance.
        /// </summary>
        /// <param name="testContainer">The test container.</param>
        /// <returns>The current IRunner instance.</returns>
        [Obsolete]
        IRunner WithTestContainer(string testContainer);

        /// <summary>
        /// Deprecated: Use Run(testContainer, testCase) instead.
        /// Sets the name of the test container and test case to execute
        /// and returns the current IRunner object.
        /// </summary>
        /// <param name="testContainer">The test container containing the test case.</param>
        /// <param name="testCase">The test case to execute.</param>
        /// <returns>The current IRunner instance.</returns>
        [Obsolete]
        IRunner WithTestContainerAndCase(string testContainer, string testCase);

        /// <summary>
        /// Runs ASPUnit tests and returns results.
        /// </summary>
        /// <returns>
        /// An AspUnitRunner.IResults instance containing the test results.
        /// </returns>
        IResults Run();

        /// <summary>
        /// Runs tests from the ASPUnit test container and returns results.
        /// </summary>
        /// <param name="testContainer">The ASPUnit test container.</param>
        /// <returns>
        /// An AspUnitRunner.IResults instance containing the test results.
        /// </returns>
        IResults Run(string testContainer);

        /// <summary>
        /// Runs the specified ASPUnit test case and returns results.
        /// </summary>
        /// <param name="testContainer">The ASPUnit test container.</param>
        /// <param name="testCase">The ASPUnit test case.</param>
        /// <returns>
        /// An AspUnitRunner.IResults instance containing the test results.
        /// </returns>
        IResults Run(string testContainer, string testCase);

        /// <summary>
        /// Retrieves the list of ASPUnit test containers.
        /// </summary>
        /// <returns>An IEnumerable&lt;string&gt; of test container names.</returns>
        IEnumerable<string> GetTestContainers();

        /// <summary>
        /// Retrieves the list of ASPUnit test cases for the specified test container.
        /// </summary>
        /// <param name="testContainer">The test container.</param>
        /// <returns>An IEnumerable&lt;string&gt; of test case names.</returns>
        IEnumerable<string> GetTestCases(string testContainer);
    }
}
