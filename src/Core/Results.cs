namespace AspUnitRunner {
    public class Results {
        private readonly int _tests;
        private readonly int _errors;
        private readonly int _failures;
        private readonly string _details;

        /// <summary>
        /// Gets the number of tests run.
        /// </summary>
        public int Tests {
            get { return _tests; }
        }

        /// <summary>
        /// Gets the number of test errors.
        /// </summary>
        public int Errors {
            get { return _errors; }
        }

        /// <summary>
        /// Gets the number of test failures.
        /// </summary>
        public int Failures {
            get { return _failures; }
        }

        /// <summary>
        /// Gets the raw HTML test results.
        /// </summary>
        public string Details {
            get { return _details; }
        }

        internal Results(int tests, int errors, int failures, string details) {
            _tests = tests;
            _errors = errors;
            _failures = failures;
            _details = details;
        }
    }
}
