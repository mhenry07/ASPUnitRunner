namespace AspUnitRunner.Tests.Helpers {
    public class FakeTestFormatter {
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
            var summaryRow = string.Format(testSummaryRow, tests, errors, failures);
            return string.Format(htmlTestResults, summaryRow);
        }
    }
}
