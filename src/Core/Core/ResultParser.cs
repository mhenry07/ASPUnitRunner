using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AspUnitRunner.Core {
    internal class ResultParser {
        private const string HtmlElementRegex = @"<{0}\b(?<attribs>[^>]*)>(?<innerHtml>(?:.(?!<{0}\b))*)</{0}>";
        private const string DetailRowClassRegex = @"\sCLASS=""(?:error|warning|success)""";
        private const string SummaryCellAttributesRegex = @"\sCOLSPAN=3";
        private const string SummaryValuesRegex =
            @"Tests\:\s*(?<tests>\d+),\s*Errors\:\s*(?<errors>\d+),\s*Failures\:\s*(?<failures>\d+)";

        private const int NumDetailCells = 3;
        private const int NumSummaryCells = 1;

        private const int DetailTypeIndex = 0;
        private const int DetailNameIndex = 1;
        private const int DetailDescriptionIndex = 2;

        private readonly Results _results;

        public static Results Parse(string htmlResults) {
            var parser = new ResultParser(htmlResults);
            return parser.Parse();
        }

        private ResultParser(string htmlResults) {
            _results = new Results {
                Html = htmlResults,
                Details = new List<ResultDetail>()
            };
        }

        private Results Parse() {
            var tableMatches = GetElementMatches(_results.Html, "TABLE");

            foreach (Match tableMatch in tableMatches) {
                ParseTable(tableMatch);
                return _results;
            }

            throw new FormatException("Unable to parse test results.");
        }

        private void ParseTable(Match tableMatch) {
            var rowMatches = GetElementMatches(GetInnerHtml(tableMatch), "TR");

            foreach (Match rowMatch in rowMatches)
                ParseRow(rowMatch);
        }

        private void ParseRow(Match rowMatch) {
            var cellMatches = GetElementMatches(GetInnerHtml(rowMatch), "TD");

            if (IsDetailRow(rowMatch, cellMatches)) {
                var details = (IList<ResultDetail>)_results.Details;
                details.Add(ParseDetail(cellMatches));
                return;
            }

            if (IsSummaryRow(rowMatch, cellMatches))
                ParseSummary(GetInnerHtml(GetFirst(cellMatches)));
        }

        private static bool IsDetailRow(Match rowMatch, MatchCollection cellMatches) {
            var rowClassRegex = new Regex(DetailRowClassRegex, RegexOptions.IgnoreCase);

            return rowClassRegex.IsMatch(GetAttributes(rowMatch))
                && cellMatches.Count == NumDetailCells
                && IsNullOrWhitespace(GetFirstElementAttributes(cellMatches));
        }

        private static bool IsSummaryRow(Match rowMatch, MatchCollection cellMatches) {
            var cellAttribsRegex = new Regex(SummaryCellAttributesRegex, RegexOptions.IgnoreCase);

            return IsNullOrWhitespace(GetAttributes(rowMatch))
                && cellMatches.Count == NumSummaryCells
                && cellAttribsRegex.IsMatch(GetFirstElementAttributes(cellMatches));
        }

        private static ResultDetail ParseDetail(MatchCollection cellMatches) {
            var type = ParseResultType(GetText(cellMatches, DetailTypeIndex));

            return new ResultDetail(type,
                GetText(cellMatches, DetailNameIndex),
                GetText(cellMatches, DetailDescriptionIndex));
        }

        private static ResultType ParseResultType(string text) {
            return (ResultType)Enum.Parse(typeof(ResultType), text);
        }

        private void ParseSummary(string text) {
            var regex = new Regex(SummaryValuesRegex);
            foreach (Match match in regex.Matches(text)) {
                ParseSummaryValues(match);
                return;
            }

            throw new FormatException("Unable to parse test results.");
        }

        private void ParseSummaryValues(Match match) {
            _results.Tests = ParseMatchedInt(match, "tests");
            _results.Errors = ParseMatchedInt(match, "errors");
            _results.Failures = ParseMatchedInt(match, "failures");
        }

        private static int ParseMatchedInt(Match match, string matchGroupName) {
            return int.Parse(match.Groups[matchGroupName].Value);
        }

        private static MatchCollection GetElementMatches(string html, string tagName) {
            var elementRegex = GetElementRegex(tagName);
            return elementRegex.Matches(html);
        }

        private static Regex GetElementRegex(string tagName) {
            var elementPattern = string.Format(HtmlElementRegex, tagName);
            return new Regex(elementPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
        }

        private static Match GetFirst(MatchCollection elementMatches) {
            if (elementMatches.Count == 0)
                return Match.Empty;
            return elementMatches[0];
        }

        private static string GetFirstElementAttributes(MatchCollection elementMatches) {
            return GetAttributes(GetFirst(elementMatches));
        }

        private static string GetAttributes(Match elementMatch) {
            return elementMatch.Groups["attribs"].Value;
        }

        private static string GetInnerHtml(Match elementMatch) {
            return elementMatch.Groups["innerHtml"].Value;
        }

        private static string GetText(MatchCollection elementMatches, int index) {
            return GetInnerHtml(elementMatches[index]).Trim();
        }

        private static bool IsNullOrWhitespace(string str) {
            if (str == null)
                return true;
            return string.IsNullOrEmpty(str.Trim());
        }
    }
}
