<%@ LANGUAGE="VBScript" %>
<% Option Explicit %>
<!-- #include file="../ASPUnit/include/ASPUnitRunner.asp" -->
<!-- #include file="CalculatorTest.asp" -->
<!-- #include file="StringUtilityTest.asp" -->
<!-- #include file="FailureTest.asp" -->
<%
' test runner
Dim runner
Set runner = New UnitRunner
Call runner.AddTestContainer(New CalculatorTest)
Call runner.AddTestContainer(New StringUtilityTest)
Call runner.AddTestContainer(New FailureTest)

Call runner.Display()
%>