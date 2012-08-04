namespace AspUnitRunner.Core {
    /// <summary>
    /// Contains the detail for a single ASPUnit test result.
    /// </summary>
    public class ResultDetail {
        private ResultType _type;
        private string _name;
        private string _description;

        /// <summary>
        /// Gets the test result type/status.
        /// </summary>
        public ResultType Type {
            get { return _type; }
        }

        /// <summary>
        /// Gets the test name. (TestContainer.TestCase)
        /// </summary>
        public string Name {
            get { return _name; }
        }

        /// <summary>
        /// Gets the test result description.
        /// </summary>
        public string Description {
            get { return _description; }
        }

        internal ResultDetail(ResultType type, string name, string description) {
            _type = type;
            _name = name;
            _description = description;
        }
    }
}
