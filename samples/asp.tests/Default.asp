<%@ LANGUAGE="VBScript" %>
<% Option Explicit %>
<!-- #include file="../ASPUnit/include/ASPUnitRunner.asp" -->
<!-- #include file="CalculatorTest.asp" -->
<!-- #include file="StringUtilityTest.asp" -->
<%
' test runner
Dim runner
Set runner = New UnitRunner
Call runner.AddTestContainer(New CalculatorTest)
Call runner.AddTestContainer(New StringUtilTest)

Call runner.Display()
%>