To run the sample tests from a web browser:
 * Make sure IIS is running and classic ASP support is enabled
   * Also, parent paths must be enabled. This has security ramifications. As an alternative you may change the ASP include statements to use absolute virtual paths based on the location where you install the ASPUnitRunner files.
 * create a new folder called "ASPUnitRunner" in IIS
   * For example: C:\Inetpub\wwwroot\ASPUnitRunner
   * Or make a virtual ASPUnitRunner directory that points to this samples folder.
 * copy the asp, asp.tests and ASPUnit folders to the new ASPUnitRunner folder
   * Note: ASPUnit is available from http://aspunit.sourceforge.net/
 * Navigate to the following location from your web browser (adjust the server name):
   * http://server/ASPUnitRunner/asp.tests/
 * Click the "Run Tests" button

To run the sample tests from NUnit 2.5:
 * follow the above steps
 * Open the asp.NUnitTests project in Visual Studio or Visual C# Express
 * Add/update references to nunit.framework.dll (2.5).
 * Update the value of AspUnitUri in TestAsp.cs
 * If credentials are required, assign them in the Runner constructor.
 * Build asp.NUnitTests.
 * Open NUnit, open the asp.NUnitTests project and run the tests
