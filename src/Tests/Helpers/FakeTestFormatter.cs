using System;
using System.Collections.Generic;
using System.Text;
using AspUnitRunner.Core;

namespace AspUnitRunner.Tests.Helpers {
    public class FakeTestFormatter {
        private const string testDetailRow =
            @"	<TR CLASS=""{0}""><TD>{1}</TD><TD>{2}</TD><TD>{3}</TD></TR>";

        private const string testSummaryRow =
            @"	<TR><TD ALIGN=""center"" COLSPAN=3>Tests: {0}, Errors: {1}, Failures: {2}</TD></TR>";

        private const string htmlTestResults = @"
<HTML>
<HEAD>
<LINK REL=""stylesheet"" HREF=""/aspunit/include/ASPUnit.css"" MEDIA=""screen"" TYPE=""text/css"">
</HEAD>
<BODY>

			<TABLE BORDER=""1"" WIDTH=""80%"">
				<TR><TH WIDTH=""10%"">Type</TH><TH WIDTH=""20%"">Test</TH><TH WIDTH=""70%"">Description</TH></TR>
{0}
			</TABLE>

</BODY>
</HTML>
";

        public static string FormatSummary(int tests, int errors, int failures) {
            var summaryRow = FormatSummaryRow(tests, errors, failures);
            return string.Format(htmlTestResults, summaryRow);
        }

        public static string FormatResults(int tests, int errors, int failures, IEnumerable<ResultDetail> details) {
            var stringBuilder = new StringBuilder();
            foreach (var detail in details)
                stringBuilder.Append(FormatDetailRow(detail));
            stringBuilder.Append(FormatSummaryRow(tests, errors, failures));
            return string.Format(htmlTestResults, stringBuilder.ToString());
        }

        private static string FormatDetailRow(ResultDetail detail) {
            return string.Format(testDetailRow,
                GetRowClass(detail.Type), detail.Type.ToString(), detail.Name, detail.Description);
        }

        private static string FormatSummaryRow(int tests, int errors, int failures) {
            return string.Format(testSummaryRow, tests, errors, failures);
        }

        private static string GetRowClass(ResultType type) {
            switch (type) {
                case ResultType.Success:
                    return "success";
                case ResultType.Error:
                    return "error";
                case ResultType.Failure:
                    return "warning";
            }
            throw new ArgumentOutOfRangeException("type");
        }
    }
}
