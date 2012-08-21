require 'albacore'

NUGET = ".nuget/nuget.exe"
PACKAGES_DIR = "lib"
NUNIT_PACKAGE = "NUnit.Runners"
NUNIT_CONSOLE = "#{PACKAGES_DIR}/#{NUNIT_PACKAGE}/tools/nunit-console.exe"

IIS_EXPRESS_EXE = "iisexpress.exe"
IIS_EXPRESS_DIR = "IIS Express"
SITE_NAME = "AspUnitRunner.Sample.Web"
WEB_SAMPLE_DIR = "sample/Web"
WEB_PORT = "54831"
WEB_ADDRESS = "http://localhost:#{WEB_PORT}"

MAIN_NAME = "AspUnitRunner"
MAIN_SOLUTION = "src/#{MAIN_NAME}.sln"
CORE_PROJECT = "src/Core/#{MAIN_NAME}.csproj"
MAIN_TESTS = "src/Tests/AspUnitRunner.Tests.csproj"
SAMPLE_SOLUTION = "sample/AspUnitRunner.Sample.sln"
SAMPLE_TESTS = "sample/Tests.NUnit/AspUnitRunner.Sample.Tests.NUnit.csproj"

BUILD_CONFIGURATION = "Release"
BUILD_DIR = "build/" # note the ending slash to avoid conflict with :build task
DELIVERABLES_DIR = "deliverables/"

# suppress DSL deprecation errors from albacore output tasks for now
# see https://github.com/derickbailey/Albacore/issues/165
Rake.application.options.ignore_deprecate = true

Albacore.configure do |config|
	config.nunit.command = NUNIT_CONSOLE
	config.nunit.options = [ "/config:#{BUILD_CONFIGURATION}" ]
end

task :default => :test

desc "Build AspUnitRunner core and tests"
task :build => "build:main"

desc "Run AspUnitRunner tests"
task :test => "test:main"

desc "Build core project and package deliverables into .zip file"
task :package => [ "build:core", "zip:core" ]

# note: not using rake dependencies for builds because firing up multiple 
# instances of msbuild is slow and makes the build script more complex
namespace :build do
	desc "Build AspUnitRunner and sample"
	task :all => [ :main, :sample ]

	desc "Build AspUnitRunner core"
	task :core => [ "clean:core", "compile:core", "copy:core" ]

	# desc "Build AspUnitRunner core and tests"
	task :main => [ "clean:core", "compile:main", "copy:core" ]

	desc "Build AspUnitRunner sample"
	task :sample do
		build_project SAMPLE_SOLUTION
	end

	namespace :compile do
		task :core do
			build_project CORE_PROJECT
		end

		task :main do
			build_project MAIN_SOLUTION
		end
	end

	# note this only cleans the ./build directory
	namespace :clean do
		task :core do
			FileUtils.rmtree File.join(BUILD_DIR, MAIN_NAME)
		end
	end

	namespace :copy do
		output :core => BUILD_DIR do |out|
			source_dir = "src/Core/bin/#{BUILD_CONFIGURATION}"
			core_build_dir = File.join(BUILD_DIR, MAIN_NAME)

			files = []
			out.from source_dir
			out.to core_build_dir
			Dir.chdir source_dir do
				files = Dir.glob "#{MAIN_NAME}.{dll,pdb,xml}"
			end
			files.each {|file| out.file file }
		end

		directory BUILD_DIR
	end

	def build_project(project)
		msb = MSBuild.new
		msb.properties :configuration => BUILD_CONFIGURATION
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
		nunit.assemblies MAIN_TESTS
	end

	desc "Run AspUnitRunner sample tests"
	task :sample => [ "build:sample", "web:register", NUNIT_CONSOLE ] do
		nunit = NUnitTestRunner.new
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

namespace :web do
	# note that the process id from $?.pid is not the iisexpress pid
	desc "Start the AspUnitRunner sample web site in IIS Express"
	task :start do
		puts "Starting the sample web site at #{WEB_ADDRESS}"
		puts
		command = %{"#{get_iis_express_exe_path}" /path:"#{get_web_sample_path}" /port:#{WEB_PORT}}
		sh %{START "" #{command}}
	end

	# find and kill the iisexpress.exe process with the expected /port command-line argument
	desc "Stop the AspUnitRunner sample web site"
	task :stop do
		puts "Stopping the sample web site"
		puts
		process_id_text = `WMIC PROCESS WHERE "Name='#{IIS_EXPRESS_EXE}' AND CommandLine LIKE '%/port:#{WEB_PORT}%'" GET ProcessId /VALUE`
		process_id = /(?<=ProcessId=)\d+/.match(process_id_text)
		sh %{TASKKILL /PID #{process_id}}
	end

	# note that the rake start/stop tasks use the path instead of the registered site
	# however, the test:sample task depends on it (since it starts iisexpress from code)
	desc "Register the AspUnitRunner sample web site in IIS Express"
	task :register do
		add_site_to_iis_express
	end

	def get_iis_express_exe_path()
		return get_iis_express_path(IIS_EXPRESS_EXE)
	end

	def get_iis_express_path(file)
		path = build_iis_express_path("ProgramFiles", file)
		return path if File.exists?(path)

		path = build_iis_express_path("ProgramFiles(x86)", file)
		return path if File.exists?(path)

		fail "Could not find IIS Express"
	end

	def build_iis_express_path(program_files_var, file)
		return [ ENV[program_files_var], IIS_EXPRESS_DIR, file ].join('\\')
	end

	# IIS Express seems to require backslashes in path
	def get_web_sample_path()
		return backslashify(File.join(Dir.pwd, WEB_SAMPLE_DIR))
	end

	def backslashify(path)
		return path.gsub(/\//, '\\')
	end

	def add_site_to_iis_express()
		appcmd = get_iis_express_path("appcmd.exe")
		site_listing = `"#{appcmd}" list SITE "#{SITE_NAME}"`.strip
		unless site_listing.empty? then
			puts "The site \"#{SITE_NAME}\" already exists"
			puts "#{site_listing}"
			return
		end
		puts "Creating new site \"#{SITE_NAME}\" in IIS Express"
		puts
		sh %{"#{appcmd}" add SITE /name:"#{SITE_NAME}" /bindings:"http/*:#{WEB_PORT}:localhost" /physicalPath:"#{get_web_sample_path}"}
	end
end

namespace :zip do
	zip :core => DELIVERABLES_DIR do |zip|
		zip.directories_to_zip File.join(BUILD_DIR, MAIN_NAME)
		zip.output_file = "#{MAIN_NAME}.zip"
		zip.output_path = DELIVERABLES_DIR
		puts "Created " + File.join(zip.output_path, zip.output_file)
	end

	directory DELIVERABLES_DIR
end
