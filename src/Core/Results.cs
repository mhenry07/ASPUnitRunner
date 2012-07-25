namespace AspUnitRunner {
    public class Results {
        private readonly int _tests;
        private readonly int _errors;
        private readonly int _failures;
        private readonly string _details;

        public int Tests {
            get { return _tests; }
        }

        public int Errors {
            get { return _errors; }
        }

        public int Failures {
            get { return _failures; }
        }

        public string Details {
            get { return _details; }
        }

        public Results(int tests, int errors, int failures, string details) {
            _tests = tests;
            _errors = errors;
            _failures = failures;
            _details = details;
        }
    }
}
