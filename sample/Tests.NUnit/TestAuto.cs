﻿using System.Collections;
using System.Text;
using NUnit.Framework;

namespace AspUnitRunner.Sample.Tests.NUnit {
    // demonstrates retrieving test container and test case names automatically from an ASPUnit test suite
    // (one NUnit test per ASPUnit test)
    [TestFixture]
    public class TestAuto {
        // set the URL for your ASPUnit tests
        private const string AspTestUrl = "http://localhost:54831/tests/Default.asp";
        // set the site name as configured in IIS Express (defaults to name of sample web project: AspUnitRunner.Sample.Web)
        private const string AspSiteName = "AspUnitRunner.Sample.Web";

        private IisExpressServer _iisServer;

        [Test, TestCaseSource("GetAspUnitTests")]
        public void Test(string testContainer, string testCase) {
            var runner = GetConfiguredRunner();
            var results = runner.Run(testContainer, testCase);

            if (!results.Successful)
                Assert.Fail(results.Format());

            if (results.Tests == 0)
                Assert.Inconclusive("0 tests were run");
        }

        // note that this runs in a separate instance from when tests run
        public IEnumerable GetAspUnitTests() {
            var runner = GetConfiguredRunner();
            using (var iisServer = StartIisExpress()) {
                foreach (var container in runner.GetTestContainers()) {
                    foreach (var testCase in runner.GetTestCases(container)) {
                        var name = string.Format("{0}.{1}", container, testCase);
                        yield return new TestCaseData(container, testCase).SetName(name);
                    }
                }
            }
        }

        // configure encoding and credentials here
        private static IRunner GetConfiguredRunner() {
            return Runner.Create(AspTestUrl)
                .WithEncoding(Encoding.UTF8);
        }

        [TestFixtureSetUp]
        public void StartServer() {
            _iisServer = StartIisExpress();
        }

        [TestFixtureTearDown]
        public void StopServer() {
            _iisServer.Stop();
        }

        private IisExpressServer StartIisExpress() {
            var server = new IisExpressServer(AspSiteName);
            server.Start();
            return server;
        }
    }
}
