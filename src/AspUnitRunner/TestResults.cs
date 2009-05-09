using System.Text.RegularExpressions;

namespace AspUnitRunner {
    public class TestResults {
        public int Tests { get; private set; }
        public int Errors { get; private set; }
        public int Failures { get; private set; }
        public string Results { get; private set; }

        public bool IsSuccessful {
            get {
                return (Errors == 0 && Failures == 0);
            }
        }

        public TestResults(string htmlResults) {
            Results = htmlResults;
            Tests = ParseCount(@"Tests\: (\d+)", htmlResults);
            Errors = ParseCount(@"Errors\: (\d+)", htmlResults);
            Failures = ParseCount(@"Failures\: (\d+)", htmlResults);
        }

        // pattern is a Regex pattern where the first capturing group is the number to parse
        private int ParseCount(string pattern, string htmlResults) {
            Regex regex = new Regex(pattern);
            foreach (Match match in regex.Matches(htmlResults))
                return int.Parse(match.Groups[1].Value);
            return 0;
        }

        public string FormatResults() {
            return Regex.Replace(Results, @"(\s*<TR)", "\r\n$1", RegexOptions.IgnoreCase);
        }
    }
}
