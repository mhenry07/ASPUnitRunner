using System.Net;
using System.Text;

namespace AspUnitRunner {
    /// <summary>
    /// Runs ASPUnit tests from the given URL and returns test results.
    /// </summary>
    public interface IRunner {
        /// <summary>
        /// Sets the network credentials used to authenticate the request
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="credentials">The network credentials.</param>
        /// <returns>The current Runner object.</returns>
        IRunner WithCredentials(ICredentials credentials);

        /// <summary>
        /// Sets the default encoding used to encode the request and decode the
        /// response and returns the current Runner object.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The current Runner object.</returns>
        /// <remarks>A charset in the response headers will take precedence.</remarks>
        IRunner WithEncoding(Encoding encoding);

        /// <summary>
        /// Sets the name of the test container from which to run tests
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="testContainer">The test container.</param>
        /// <returns>The current Runner object.</returns>
        IRunner WithTestContainer(string testContainer);

        /// <summary>
        /// Sets the name of the test container and test case to execute
        /// and returns the current Runner object.
        /// </summary>
        /// <param name="testContainer">The test container containing the test case.</param>
        /// <param name="testCase">The test case to execute.</param>
        /// <returns>The current Runner object.</returns>
        IRunner WithTestContainerAndCase(string testContainer, string testCase);

        /// <summary>
        /// Runs ASPUnit tests and returns results.
        /// </summary>
        /// <returns>An AspUnitRunner.IResults containing the test results.</returns>
        IResults Run();
    }
}
