require 'albacore'

MAIN_SOLUTION = "src/AspUnitRunner.sln"
CORE_PROJECT = "src/Core/AspUnitRunner.csproj"
SAMPLE_SOLUTION = "sample/AspUnitRunner.Sample.sln"

desc "Build AspUnitRunner core and tests"
task :build => "build:main"

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
