﻿<%
Class FailureTest
	Public Function TestCaseNames()
		TestCaseNames = Array("ExpectFailure", "ExpectError")
	End Function

	Public Sub SetUp()
	End Sub

	Public Sub TearDown()
	End Sub

	Public Sub ExpectFailure(testResult)
		Call testResult.AddFailure("Expected failure")
	End Sub

	Public Sub ExpectError(testResult)
		Dim x
		x = CInt("not a number")
	End Sub
End Class
%>
