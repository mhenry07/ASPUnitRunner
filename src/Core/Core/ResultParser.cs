using System;
using System.Text.RegularExpressions;

namespace AspUnitRunner.Core {
    internal class ResultParser {
        private const string TestSummaryRegex =
            @"Tests\:\s*(?<tests>\d+),\s*Errors\:\s*(?<errors>\d+),\s*Failures\:\s*(?<failures>\d+)";

        private readonly Results _results;

        private Results Results {
            get { return _results; }
        }

        public static Results Parse(string htmlResults) {
            var parser = new ResultParser(htmlResults);
            parser.ParseSummary();
            return parser.Results;
        }

        private ResultParser(string htmlResults) {
            _results = new Results {
                Html = htmlResults,
                Details = new ResultDetail[] { }
            };
        }

        private void ParseSummary() {
            var regex = new Regex(TestSummaryRegex);
            foreach (Match match in regex.Matches(_results.Html)) {
                ParseValues(match);
                return;
            }

            throw new FormatException("Unable to parse test results.");
        }

        private void ParseValues(Match match) {
            _results.Tests = ParseMatchedInt(match, "tests");
            _results.Errors = ParseMatchedInt(match, "errors");
            _results.Failures = ParseMatchedInt(match, "failures");
        }

        private static int ParseMatchedInt(Match match, string matchGroupName) {
            return int.Parse(match.Groups[matchGroupName].Value);
        }
    }
}
