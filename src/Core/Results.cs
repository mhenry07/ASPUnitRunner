namespace AspUnitRunner.Core {
    public class Results {
        public int Tests { get; private set; }
        public int Errors { get; private set; }
        public int Failures { get; private set; }
        public string Details { get; private set; }

        public Results(int tests, int errors, int failures, string details) {
            Tests = tests;
            Errors = errors;
            Failures = failures;
            Details = details;
        }
    }
}
