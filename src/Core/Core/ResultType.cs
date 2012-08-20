namespace AspUnitRunner.Core {
    /// <summary>
    /// An enumeration of test result types/statuses.
    /// </summary>
    public enum ResultType {
        /// <summary>
        /// The test succeeded.
        /// </summary>
        Success,

        /// <summary>
        /// The test resulted in an error.
        /// </summary>
        Error,

        /// <summary>
        /// The test failed.
        /// </summary>
        Failure
    }
}
