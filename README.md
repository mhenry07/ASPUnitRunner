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
		* Note: You may first need to configure the AspUnitRunner.Sample.Web 
		  site in IIS Express. The easiest way is to open the sample solution
		  (sample/AspUnitRunner.Sample.sln) in Visual Studio.
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
