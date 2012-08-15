using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AspUnitRunner.Core.Html;

namespace AspUnitRunner.Core {
    internal class ResultParser : IResultParser {
        private const string SummaryValuesRegex =
            @"Tests\:\s*(?<tests>\d+),\s*Errors\:\s*(?<errors>\d+),\s*Failures\:\s*(?<failures>\d+)";

        private const int NumDetailCells = 3;
        private const int NumSummaryCells = 1;

        private const int DetailTypeIndex = 0;
        private const int DetailNameIndex = 1;
        private const int DetailDescriptionIndex = 2;

        private readonly IHtmlDocumentFactory _htmlDocumentFactory;
        private Results _results;

        public ResultParser(IHtmlDocumentFactory htmlDocumentFactory) {
            _htmlDocumentFactory = htmlDocumentFactory;
        }

        public Results Parse(string htmlResults) {
            _results = new Results {
                Html = htmlResults,
                Details = new List<ResultDetail>()
            };

            var doc = _htmlDocumentFactory.Create(_results.Html);
            var tables = doc.GetElementsByTagName("TABLE");

            foreach (var table in tables) {
                ParseTable(table);
                return _results;
            }

            throw new FormatException("Unable to parse test results.");
        }

        private void ParseTable(IHtmlElement table) {
            foreach (var row in table.GetElementsByTagName("TR"))
                ParseRow(row);
        }

        private void ParseRow(IHtmlElement row) {
            var cells = row.GetElementsByTagName("TD");

            if (IsDetailRow(row, cells)) {
                var details = (IList<ResultDetail>)_results.Details;
                details.Add(ParseDetail(cells));
                return;
            }

            if (IsSummaryRow(row, cells))
                ParseSummary(cells.First.Text);
        }

        private static bool IsDetailRow(IHtmlElement row, IHtmlCollection cells) {
            return HasDetailRowClass(row)
                && cells.Length == NumDetailCells
                && cells.First.Attributes.Count == 0;
        }

        private static bool HasDetailRowClass(IHtmlElement row) {
            var classNames = new List<string> { "error", "warning", "success" };
            return !string.IsNullOrEmpty(row.ClassName)
                && classNames.Contains(row.ClassName);
        }

        private static bool IsSummaryRow(IHtmlElement row, IHtmlCollection cells) {
            return row.Attributes.Count == 0
                && cells.Length == NumSummaryCells
                && HasSummaryCellAttributes(cells.First);
        }

        private static bool HasSummaryCellAttributes(IHtmlElement cell) {
            var attributes = cell.Attributes;
            return attributes.ContainsKey("COLSPAN") && attributes["COLSPAN"] == "3";
        }

        private static ResultDetail ParseDetail(IHtmlCollection cells) {
            var type = ParseResultType(cells[DetailTypeIndex].Text);

            return new ResultDetail(type,
                cells[DetailNameIndex].Text,
                cells[DetailDescriptionIndex].Text);
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
    }
}
