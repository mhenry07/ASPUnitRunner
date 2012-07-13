namespace AspUnitRunner.Tests {
    internal class FakeTestFormatter {
        public static string FormatSummary(int tests, int errors, int failures) {
            return string.Format("<html><body><table><tr>Tests: {0}, Errors: {1}, Failures: {2}</tr></table></body></html>",
                tests, errors, failures);
        }
    }
}
