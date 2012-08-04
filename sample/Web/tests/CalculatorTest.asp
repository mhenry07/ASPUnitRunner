<!-- #include virtual="/includes/Calculator.inc.asp" -->
<%
Class CalculatorTest
	Private m_calculator

	Public Function TestCaseNames()
		TestCaseNames = Array("Adding_zeros_should_return_zero", "Adding_ones_should_return_two")
	End Function

	Public Sub SetUp()
		Set m_calculator = New Calculator
	End Sub

	Public Sub TearDown()
	End Sub

	Public Sub Adding_zeros_should_return_zero(testResult)
		Dim actual
		actual = m_calculator.Add(0, 0)
		Call testResult.AssertEquals(0, actual, "")
	End Sub

	Public Sub Adding_ones_should_return_two(testResult)
		Dim actual
		actual = m_calculator.Add(1, 1)
		Call testResult.AssertEquals(2, actual, "")
	End Sub
End Class
%>
