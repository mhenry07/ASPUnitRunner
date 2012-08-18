require 'albacore'

NUGET = ".nuget/nuget.exe"
PACKAGES_DIR = "lib"
NUNIT_PACKAGE = "NUnit.Runners"
NUNIT_CONSOLE = "#{PACKAGES_DIR}/#{NUNIT_PACKAGE}/tools/nunit-console.exe"

MAIN_SOLUTION = "src/AspUnitRunner.sln"
CORE_PROJECT = "src/Core/AspUnitRunner.csproj"
MAIN_TESTS = "src/Tests/AspUnitRunner.Tests.csproj"
SAMPLE_SOLUTION = "sample/AspUnitRunner.Sample.sln"
SAMPLE_TESTS = "sample/Tests.NUnit/AspUnitRunner.Sample.Tests.NUnit.csproj"

task :default => :test

desc "Build AspUnitRunner core and tests"
task :build => "build:main"

desc "Run AspUnitRunner tests"
task :test => "test:main"

# note: not using rake dependencies for builds because firing up multiple 
# instances of msbuild is slow and makes the build script more complex
namespace :build do
	desc "Build AspUnitRunner and sample"
	task :all => [ :main, :sample ]

	desc "Build AspUnitRunner core"
	task :core do
		build_project CORE_PROJECT
	end

	# desc "Build AspUnitRunner core and tests"
	task :main do
		build_project MAIN_SOLUTION
	end

	desc "Build AspUnitRunner sample"
	task :sample do
		build_project SAMPLE_SOLUTION
	end

	def build_project(project)
		msb = MSBuild.new
		msb.properties :configuration =>  :Debug
		msb.targets :Clean, :Build
		msb.solution = project
		msb.verbosity = "minimal"
		#msb.log_level = :verbose
		msb.execute
	end
end

namespace :test do
	desc "Run AspUnitRunner and sample tests"
	task :all => [ :main, :sample ]

	# desc "Run AspUnitRunner tests"
	nunit :main => [ "build:main", NUNIT_CONSOLE ] do |nunit|
		nunit.command = NUNIT_CONSOLE
		nunit.assemblies MAIN_TESTS
	end

	desc "Run AspUnitRunner sample tests"
	task :sample => [ "build:sample", NUNIT_CONSOLE ] do
		nunit = NUnitTestRunner.new
		nunit.command = NUNIT_CONSOLE
		nunit.assemblies SAMPLE_TESTS
		begin
			nunit.execute
		rescue RuntimeError
			puts "Note: test failures in FailureTest were expected"
		end
	end

	# use NuGet to get NUnit.Runners
	# note: if nuget.exe was missing, a build prereq should've grabbed it
	file NUNIT_CONSOLE do
		sh %{#{NUGET} install #{NUNIT_PACKAGE} -o #{PACKAGES_DIR} -x}
	end
end
