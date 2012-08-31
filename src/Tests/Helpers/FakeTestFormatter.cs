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

        private const string htmlSelector = @"
<HTML>
<HEAD>
<LINK REL=""stylesheet"" HREF=""/aspunit/include/ASPUnit.css"" MEDIA=""screen"" TYPE=""text/css"">
<SCRIPT>
function ComboBoxUpdate(strSelectorFrameSrc, strSelectorFrameName)
{{
	document.frmSelector.action = strSelectorFrameSrc;
	document.frmSelector.target = strSelectorFrameName;
	document.frmSelector.submit();
}}
</SCRIPT>
</HEAD>
<BODY>
		<FORM NAME=""frmSelector"" ACTION=""{0}?UnitRunner=results"" TARGET=""results"" METHOD=POST>
			<TABLE WIDTH=""80%"">
				<TR>
					<TD ALIGN=""right"">Test:</TD>
					<TD>
						<SELECT NAME=""cboTestContainers"" OnChange=""ComboBoxUpdate('{0}?UnitRunner=selector', 'selector');"">
						<OPTION>All Test Containers{1}
						</SELECT>
					</TD>
					<TD ALIGN=""right"">Test Method:</TD>
					<TD>
						<SELECT NAME=""cboTestCases"">
						<OPTION>All Test Cases{2}
						</SELECT>
					<TD>
						<INPUT TYPE=""checkbox"" NAME=""chkShowSuccess""> Show Passing Tests</INPUT>
					</TD>
					</TD>
					<TD>
						<INPUT TYPE=""Submit"" NAME=""cmdRun"" VALUE=""Run Tests"">
					</TD>
				</TR>
			</TABLE>
		</FORM>
</BODY>
</HTML>
";

        public static string FormatSummary(int tests, int errors, int failures) {
            var summaryRow = FormatSummaryRow(tests, errors, failures);
            return string.Format(htmlTestResults, summaryRow);
        }

        public static string FormatResults(int tests, int errors, int failures, IEnumerable<IResultDetail> details) {
            var stringBuilder = new StringBuilder();
            foreach (var detail in details)
                stringBuilder.Append(FormatDetailRow(detail));
            stringBuilder.Append(FormatSummaryRow(tests, errors, failures));
            return string.Format(htmlTestResults, stringBuilder.ToString());
        }

        public static string FormatSelector(IEnumerable<string> testContainers, IEnumerable<string> testCases) {
            const string testSuitePath = "/tests/Default.asp";
            return string.Format(htmlSelector, testSuitePath,
                FormatOptions(testContainers), FormatOptions(testCases));
        }

        private static string FormatDetailRow(IResultDetail detail) {
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

        private static string FormatOptions(IEnumerable<string> options) {
            if (options == null)
                return "";
            var stringBuilder = new StringBuilder();
            foreach (var option in options)
                stringBuilder.AppendFormat("<OPTION>{0}", option);
            return stringBuilder.ToString();
        }
    }
}
