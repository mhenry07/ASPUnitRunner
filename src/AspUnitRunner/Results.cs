using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AspUnitRunner {
    public class Results {
        public int Tests { get; private set; }
        public int Errors { get; private set; }
        public int Failures { get; private set; }
        public string Details { get; private set; }

        public Results(string htmlResults) {
            ParseResults(htmlResults);
            Details = htmlResults;
        }

        private void ParseResults(string htmlResults) {
            Regex regex = new Regex(@"Tests\:\s*(?<tests>\d+),\s*Errors\:\s*(?<errors>\d+),\s*Failures\:\s*(?<failures>\d+)");
            foreach (Match match in regex.Matches(htmlResults)) {
                Tests = int.Parse(match.Groups["tests"].Value);
                Errors = int.Parse(match.Groups["errors"].Value);
                Failures = int.Parse(match.Groups["failures"].Value);
            }
        }
    }
}
