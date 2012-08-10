﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using AspUnitRunner.Core.Html;

namespace AspUnitRunner.Core {
    internal class ResultParser {
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
            var doc = new HtmlDoc(_results.Html);
            var tables = doc.GetDescendants("TABLE");

            foreach (var table in tables) {
                ParseTable(table);
                return _results;
            }

            throw new FormatException("Unable to parse test results.");
        }

        private void ParseTable(HtmlElement table) {
            foreach (var row in table.GetDescendants("TR"))
                ParseRow(row);
        }

        private void ParseRow(HtmlElement row) {
            var cells = row.GetDescendants("TD");

            if (IsDetailRow(row, cells)) {
                var details = (IList<ResultDetail>)_results.Details;
                details.Add(ParseDetail(cells));
                return;
            }

            if (IsSummaryRow(row, cells))
                ParseSummary(cells.First.Text);
        }

        private static bool IsDetailRow(HtmlElement row, HtmlElementCollection cells) {
            var rowClassRegex = new Regex(DetailRowClassRegex, RegexOptions.IgnoreCase);

            return rowClassRegex.IsMatch(row.Attributes)
                && cells.Count == NumDetailCells
                && IsNullOrWhitespace(cells.First.Attributes);
        }

        private static bool IsSummaryRow(HtmlElement row, HtmlElementCollection cells) {
            var cellAttribsRegex = new Regex(SummaryCellAttributesRegex, RegexOptions.IgnoreCase);

            return IsNullOrWhitespace(row.Attributes)
                && cells.Count == NumSummaryCells
                && cellAttribsRegex.IsMatch(cells.First.Attributes);
        }

        private static ResultDetail ParseDetail(HtmlElementCollection cells) {
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

        private static bool IsNullOrWhitespace(string str) {
            if (str == null)
                return true;
            return string.IsNullOrEmpty(str.Trim());
        }
    }
}
