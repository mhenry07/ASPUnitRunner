using System;
using System.Text.RegularExpressions;

namespace AspUnitRunner.Core {
    internal class ResultParser {
        private const string TestResultRegex =
            @"Tests\:\s*(?<tests>\d+),\s*Errors\:\s*(?<errors>\d+),\s*Failures\:\s*(?<failures>\d+)";

        public static Results Parse(string htmlResults) {
            var regex = new Regex(TestResultRegex);
            foreach (Match match in regex.Matches(htmlResults)) {
                return ParseValues(htmlResults, match);
            }

            throw new FormatException("Unable to parse test results.");
        }

        private static Results ParseValues(string htmlResults, Match match) {
            var tests = ParseMatchedInt(match, "tests");
            var errors = ParseMatchedInt(match, "errors");
            var failures = ParseMatchedInt(match, "failures");

            return new Results(tests, errors, failures, htmlResults);
        }

        private static int ParseMatchedInt(Match match, string matchGroupName) {
            return int.Parse(match.Groups[matchGroupName].Value);
        }
    }
}
