using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AspUnitRunner {
    public class Results {
        public int Errors { get; private set; }
        public int Failures { get; private set; }

        public Results(string htmlResults) {
            ParseResults(htmlResults);
        }

        private void ParseResults(string htmlResults) {
            Regex regex = new Regex(@"Errors\:\s*(?<errors>\d+),\s*Failures\:\s*(?<failures>\d+)");
            foreach (Match match in regex.Matches(htmlResults)) {
                Errors = int.Parse(match.Groups["errors"].Value);
                Failures = int.Parse(match.Groups["failures"].Value);
            }
        }
    }
}
