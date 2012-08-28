# ASPUnitRunner

ASPUnitRunner is a library to allow running ASPUnit tests for classic ASP 
from NUnit.

Basically, it is an alternative to running ASPUnit tests from a web browser 
and could theoretically be used to allow running tests from a continuous 
integration server.

ASPUnit is an xUnit framework for classic ASP/VBScript. See 
<http://aspunit.sourceforge.net/>. There is no direct affiliation between 
ASPUnitRunner and ASPUnit.

Mike Henry  
<http://www.mikehenry.name/>


## Usage

See the sample project for an example.

The following assumes you already have a classic ASP web site or application
with ASPUnit tests.

* Get AspUnitRunner.dll and reference it from your .NET test project (NUnit
  or other framework).
	* For the latest, get the source and build it yourself (see [Development
	  Environment][] below)
	* Or download binaries from
	  <https://github.com/mhenry07/ASPUnitRunner/downloads>
* From your test case method in .NET, create a new Runner object via 
  `Runner.Create("http://localhost:port/path/to/tests")`.
	* Specify the web address of your ASPUnit test suite.
	* Configure the runner fluently by chaining zero or more of the following
	  methods: `WithCredentials`, `WithEncoding`, `WithTestContainer` and 
	  `WithTestContainerAndCase` to your *Runner.Create()* call.
* Call the `Run` method. This will run the ASPUnit tests and return a Results 
  object containing your test results.
* Assert that the `Successful` property of the Results object is true.
* Optionally, use the `Format` method for the assertion failure message.
* Note that to run your tests, your web server will have to be running when 
  tests are executing.
* As a security reminder, you probably don't want to publish your unit tests 
  nor the ASPUnit directory when you deploy your application to a production 
  web server.


### NUnit Example

```csharp
	using AspUnitRunner;
	//...
	
	[Test]
	public void CalculatorTest() {
		// path to your ASPUnit test suite
		var runner = Runner.Create("http://localhost:54831/tests/Default.asp")
			.WithCredentials(new NetworkCredential("username", "password"))
			.WithEncoding(Encoding.UTF8)
			.WithTestContainer("CalculatorTest"); // run all tests within CalculatorTest
	
		var results = runner.Run();
	
		Assert.That(results.Successful, results.Format());
	}
```


## Development Environment

* [Microsoft .NET Framework 4][] (although the core project targets .NET
  Framework 2.0)
	* .NET 3.5 or 4.5 should work but may require some tweaks
* Enable NuGet package restore
	* From Visual Studio: Tools > Options... > Package Manager > General
		* Under Package Restore, enable *Allow NuGet to download missing 
		  packages during build*
	* Or, set the environment variable *EnableNuGetPackageRestore* to "true".
* Install Ruby and RubyGems
	* [RubyInstaller for Windows](http://rubyinstaller.org/) makes it easy
* Install [RAKE – Ruby Make](http://rake.rubyforge.org/)
	* `gem install rake`
* Install [Albacore](http://albacorebuild.net/)
	* `gem install albacore`

Then, you can build the main project and run tests via `rake`.

### Optional

* [IIS Express][] to run the sample
	* Tested with IIS 8.0 Express (which is included with Visual Web 
	  Developer 2010 SP1)
	* Run `rake test:sample` to build and run the sample tests.
		* A new site "AspUnitRunner.Sample.Web" will be added to IIS Express
		  if it does not already exist.
	* To view the sample web site, run `rake web:start` to start the web 
	  site. Then, navigate to the indicated URL from your web browser.
	* Other web servers (namely IIS) which support classic ASP should work 
	  but will likely require modifying the sample.
* [Visual Studio 2010][] SP1 or [Visual Web Developer Express 2010][] SP1
	* Or newer

*Tip:* Use the [Microsoft Web Platform Installer][] to simplify installation 
of Microsoft tools.

[Microsoft .NET Framework 4]: http://www.microsoft.com/en-us/download/details.aspx?id=17851
[Visual Web Developer Express 2010 SP1]: http://www.microsoft.com/visualstudio/en-us/products/2010-editions/visual-web-developer-express
[Visual Studio 2010]: http://www.microsoft.com/visualstudio/en-us/products/2010-editions
[IIS Express]: http://learn.iis.net/page.aspx/860/iis-express/
[Microsoft Web Platform Installer]: http://www.microsoft.com/web/downloads/platform.aspx
