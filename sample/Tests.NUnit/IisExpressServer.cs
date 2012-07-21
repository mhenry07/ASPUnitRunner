using System.Diagnostics;
using System.IO;

namespace AspUnitRunner.Sample.Tests.NUnit {
    // based on http://www.reimers.dk/jacob-reimers-blog/testing-your-web-application-with-iis-express-and-unit-tests
    public class IisExpressServer {
        private readonly string _siteName;
        private Process _iisProcess;

        public IisExpressServer(string siteName) {
            _siteName = siteName;
        }

        public void Start() {
            var startInfo = new ProcessStartInfo {
                Arguments = string.Format("/site:{0}", _siteName),
                UseShellExecute = false
            };

            startInfo.FileName = GetIisExpressExecPath(startInfo);

            try {
                _iisProcess = new Process { StartInfo = startInfo };
                _iisProcess.Start();
                _iisProcess.WaitForExit();
            } catch {
                Stop();
                throw;
            }
        }

        // assumes IIS Express is installed at %programfiles(x86)%\IIS Express\iisexpress.exe or
        // %programfiles%\IIS Express\iisexpress.exe
        private static string GetIisExpressExecPath(ProcessStartInfo startInfo) {
            var programFiles = GetProgramFilesDir(startInfo);

            return Path.Combine(programFiles, @"IIS Express\iisexpress.exe");
        }

        private static string GetProgramFilesDir(ProcessStartInfo startInfo) {
            var programFiles = startInfo.EnvironmentVariables["programfiles(x86)"];
            if (string.IsNullOrEmpty(programFiles))
                return startInfo.EnvironmentVariables["programfiles"];
            return programFiles;
        }

        public void Stop() {
            if (_iisProcess == null)
                return;
            if (!_iisProcess.HasExited)
                _iisProcess.CloseMainWindow();
            _iisProcess.Dispose();
            _iisProcess = null;
        }
    }
}
