# AspUnitRunner Sample:


To run the sample via Visual Studio 2010 SP1 (or Visual Web Developer 2010 
Express):

* Open sample/AspUnitRunner.Sample.sln in Visual Studio
	- If you receive a warning that the local IIS URL has not been 
	  configured, click Yes to create the virtual directory in IIS Express.
* Compile AspUnitRunner.Sample.Tests.NUnit
* Open AspUnitRunner.Sample.Tests.NUnit.dll in NUnit GUI or command-line and 
  run tests
	- The .dll should be located at Tests.NUnit\bin\Debug
	- IIS Express should run automatically
* To run manually, select the AspUnitRunner.Sample.Web project in the 
  solution explorer and start without debugging (Ctrl+F5)
	- To view sample in browser, navigate to <http://localhost:54831> and 
	  click Run Tests
* Note: The sample uses a web application instead of web site project because
  it seems to be a little more portable regarding IIS Express.


To run the sample via IIS Express command-line:

* Run: `iisexpress.exe /path:<full_path_to_sample_folder> /port:54831`
* (Compile Tests.NUnit project)
* Open AspUnitRunner.Sample.Tests.NUnit.dll in NUnit GUI or command-line and 
  run tests
* To view sample, navigate to <http://localhost:54831>
* Note: If you need to use a different port, update `AspTestUrl` in 
  Tests.NUnit\TestAsp.cs and recompile Tests.NUnit.


To run the sample via WebMatrix 2:

* Open WebMatrix
	- Open site > Folder as Site
	- Locate the sample folder and click Select Folder
	- If the site URL is not http://localhost:54831, then do one of the 
	  following:
		+ Click Settings, update the URL to http://localhost:54831 and tab 
		  away from the field to apply the change and restart IIS Express
		+ Or, update `AspTestUrl` in Tests.NUnit\TestAsp.cs
* Compile AspUnitRunner.Sample.Tests.NUnit project
* Open AspUnitRunner.Sample.Tests.NUnit.dll in NUnit GUI or command-line and 
  run tests
* To view sample, click Run from WebMatrix


To run the sample from non-root IIS/IIS Express directory:  
(Your application root directory is not /. E.g. http://localhost/MyApp)

* For IIS, make sure it's running and classic ASP support is installed and
  enabled
* Enable parent paths
	- For IIS Express (in 
	  %UserProfile%\Documents\IISExpress\config\applicationhost.config):
		+ Locate `<system.webServer>`
		+ Add `enableParentPaths="true"` attribute to `<asp>` element
	- Note that this has security ramifications. As an alternative, you may 
	  change the ASP include statements to use absolute virtual paths based 
	  on the virtual directory for AspUnitRunner.
		+ E.g. /AspUnitRuner/ASPUnit/include/ASPUnitRunner.asp
* Convert `#include` statements in sample .asp files to relative paths
	- E.g. `<-- #include virtual="/ASPUnit/include/ASPUnitRunner.asp" -->` 
	  becomes `<-- #include file="../ASPUnit/include/ASPUnitRunner.asp" -->`
	- Currently, .asp files in the tests folder would need to be updated
* Copy the sample content to the desired application directory for IIS or IIS
  Express and configure application, virtual directory, port, etc.
	- To set up a virtual directory for IIS Express (via the 
	  applicationhost.config file):
		+ Locate the AspUnitRunner.Sample.Web site under 
		  configuration/system.applicationHost/sites
		+ Under application, set the virtualDirectory path to e.g. 
		  "/AspUnitRunner"
* If credentials are required, configure them by calling `WithCredentials` 
  after the `Runner.Create()` call in Tests.NUnit\TestAsp.cs. E.g.

		Runner.Create()
			.WithCredentials(new NetworkCredential("username", "password"));

* Update `AspTestUrl` in Tests.NUnit\TestAsp.cs to point to your ASPUnit test
  fixture and recompile AspUnitRunner.Sample.Tests.NUnit
* Open AspUnitRunner.Sample.Tests.NUnit.dll in NUnit GUI or command-line and 
  run tests
