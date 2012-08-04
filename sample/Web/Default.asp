<%@ LANGUAGE="VBScript" CODEPAGE=65001 %>
<%
Option Explicit
Response.CodePage = 65001
Response.Charset = "UTF-8"
%>
<!-- #include file="includes/Calculator.inc.asp" -->
<!-- #include file="includes/StringUtility.inc.asp" -->
<%
Dim calc, stringUtil
Set calc = New Calculator
Set stringUtil = New StringUtility
%>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html;charset=UTF-8">
	<title>AspUnitRunner sample</title>
</head>
<body>
	<h1>AspUnitRunner sample</h1>
	<p>This page is a sample page showing the tested code in use. Run the ASPUnit tests <a href="tests">here</a>.</p>
	<h2>Example usage:</h2>
	<dl>
		<dt>Calculator.Add example:</dt>
		<dd>5 + 5 = <%= calc.Add(5, 5) %></dd>
		<dt>StringUtility.ToLower example:</dt>
		<dd>XYZ to lower case is: <%= stringUtil.ToLower("XYZ") %></dd>
	</dl>
</body>
</html>
