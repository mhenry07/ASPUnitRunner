﻿using System.Collections.Specialized;
using System.Net;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner;
using AspUnitRunner.Core;
using AspUnitRunner.Tests.Helpers;

namespace AspUnitRunner.Tests.Unit.Core {
    [TestFixture]
    public class TestAspRunner {
        private const string AddressField = "_address";
        private const string TestContainerField = "_testContainer";
        private const string TestCaseField = "_testCase";

        private IAspClient _client;
        private IResultParser _resultParser;
        private ISelectorParser _selectorParser;

        [SetUp]
        public void SetUp() {
            _client = MockRepository.GenerateMock<IAspClient>();
            _resultParser = MockRepository.GenerateStub<IResultParser>();
            _selectorParser = MockRepository.GenerateStub<ISelectorParser>();
        }

        [Test]
        public void New_Runner_should_have_default_configuration() {
            var runner = CreateRunner();

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(AspRunner.AllTestContainers));
            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(AspRunner.AllTestCases));
            _client.AssertWasNotCalled(c => c.Credentials = Arg<ICredentials>.Is.NotNull);
        }

        [Test]
        public void WithAddress_should_set_address() {
            const string address = "http://path/to/test-runner";
            var runner = CreateRunner()
                .WithAddress(address);

            Assert.That(runner.GetField(AddressField), Is.EqualTo(address));
        }

        [Test]
        public void Run_should_return_expected_results() {
            var expectedHtml = "<HTML></HTML>";
            var expectedResults = new Results();
            _client.Stub(c => c.PostRequest(Arg<string>.Is.Anything, Arg<NameValueCollection>.Is.Anything))
                .Return(expectedHtml);
            _resultParser.Stub(p => p.Parse(expectedHtml))
                .Return(expectedResults);

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var results = runner.Run();
            Assert.That(results, Is.EqualTo(expectedResults));
        }

        [Test]
        public void Run_should_post_request_to_expected_address_with_all_test_containers() {
            var expectedData = new NameValueCollection {
                { "cboTestContainers", AspRunner.AllTestContainers },
                { "cboTestCases", AspRunner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var results = runner.Run();

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg.Is("http://path/to/test-runner?UnitRunner=results"),
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Run_with_container_should_post_request_with_test_container() {
            const string testContainer = "TestContainer";
            var expectedData = new NameValueCollection {
                { "cboTestContainers", testContainer },
                { "cboTestCases", AspRunner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner")
                .WithTestContainer(testContainer);
            var results = runner.Run();

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Run_with_test_container_and_case_should_post_request_with_test_container_and_test_case() {
            const string testContainer = "TestContainer";
            const string testCase = "TestCase";
            var expectedData = new NameValueCollection {
                { "cboTestContainers", testContainer },
                { "cboTestCases", testCase },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner")
                .WithTestContainerAndCase(testContainer, testCase);
            var results = runner.Run();

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Run_container_should_post_request_with_test_container() {
            const string testContainer = "TestContainer";
            var expectedData = new NameValueCollection {
                { "cboTestContainers", testContainer },
                { "cboTestCases", AspRunner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var results = runner.Run(testContainer);

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Run_null_container_should_post_request_with_all_test_containers() {
            var expectedData = new NameValueCollection {
                { "cboTestContainers", AspRunner.AllTestContainers },
                { "cboTestCases", AspRunner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var results = runner.Run(null);

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Run_empty_container_should_post_request_with_all_test_containers() {
            var expectedData = new NameValueCollection {
                { "cboTestContainers", AspRunner.AllTestContainers },
                { "cboTestCases", AspRunner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var results = runner.Run("");

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Run_test_case_should_post_request_with_test_container_and_test_case() {
            const string testContainer = "TestContainer";
            const string testCase = "TestCase";
            var expectedData = new NameValueCollection {
                { "cboTestContainers", testContainer },
                { "cboTestCases", testCase },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var results = runner.Run(testContainer, testCase);

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Run_test_case_for_all_containers_should_throw_exception() {
            var runner = CreateRunner();

            Assert.That(
                () => runner.Run(AspRunner.AllTestContainers, "TestCase"),
                Throws.InstanceOf<System.ArgumentException>());
        }

        [Test]
        public void Run_null_test_case_should_post_request_with_test_container_and_all_test_cases() {
            const string testContainer = "TestContainer";
            var expectedData = new NameValueCollection {
                { "cboTestContainers", testContainer },
                { "cboTestCases", AspRunner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var results = runner.Run(testContainer, null);

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void Run_empty_test_case_should_post_request_with_test_container_and_all_test_cases() {
            const string testContainer = "TestContainer";
            var expectedData = new NameValueCollection {
                { "cboTestContainers", testContainer },
                { "cboTestCases", AspRunner.AllTestCases },
                { "cmdRun", "Run Tests"}
            };

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var results = runner.Run(testContainer, "");

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg<string>.Is.Anything,
                    Arg<NameValueCollection>.Matches(arg => arg.SequenceEqual(expectedData))));
        }

        [Test]
        public void GetTestContainers_should_return_expected_collection() {
            const string expectedHtml = "<HTML></HTML>";
            var expectedContainers = new[] {
                "FirstContainer",
                "SecondContainer"
            };
            _client.Stub(c => c.PostRequest(Arg<string>.Is.Anything, Arg<NameValueCollection>.Is.Anything))
                .Return(expectedHtml);
            _selectorParser.Stub(p => p.ParseContainers(expectedHtml))
                .Return(expectedContainers);

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var testContainers = runner.GetTestContainers();
            Assert.That(testContainers, Is.EqualTo(expectedContainers));
        }

        [Test]
        public void GetTestContainers_should_post_request_to_expected_address() {
            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var testContainers = runner.GetTestContainers();

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg.Is("http://path/to/test-runner?UnitRunner=selector"),
                    Arg<NameValueCollection>.Is.Anything));
        }

        [Test]
        public void GetTestCases_should_return_expected_collection() {
            const string testContainer = "Container";
            const string expectedHtml = "<HTML></HTML>";
            var expectedTestCases = new[] {
                "FirstTest",
                "SecondTest"
            };
            _client.Stub(c => c.PostRequest(Arg<string>.Is.Anything, Arg<NameValueCollection>.Is.Anything))
                .Return(expectedHtml);
            _selectorParser.Stub(p => p.ParseTestCases(expectedHtml))
                .Return(expectedTestCases);

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var testCases = runner.GetTestCases(testContainer);
            Assert.That(testCases, Is.EqualTo(expectedTestCases));
        }

        [Test]
        public void GetTestCases_should_post_request_with_expected_address_and_container() {
            const string testContainer = "Container";

            var runner = CreateRunner()
                .WithAddress("http://path/to/test-runner");
            var testContainers = runner.GetTestCases(testContainer);

            _client.AssertWasCalled(c =>
                c.PostRequest(
                    Arg.Is("http://path/to/test-runner?UnitRunner=selector"),
                    Arg<NameValueCollection>.Matches(arg => arg["cboTestContainers"] == testContainer)));
        }

        [Test]
        public void WithCredentials_should_set_client_credentials() {
            var credentials = new NetworkCredential("username", "password");

            var runner = CreateRunner()
                .WithCredentials(credentials);

            _client.AssertWasCalled(c => c.Credentials = credentials);
        }

        [Test]
        public void WithEncoding_should_set_client_encoding() {
            var encoding = Encoding.UTF8;

            var runner = CreateRunner()
                .WithEncoding(encoding);

            _client.AssertWasCalled(c => c.Encoding = encoding);
        }

        [Test]
        public void WithTestContainerAndCase_with_test_case_for_all_containers_should_throw_exception() {
            var runner = CreateRunner();

            Assert.That(
                () => runner.WithTestContainerAndCase(AspRunner.AllTestContainers, "TestCase"),
                Throws.InstanceOf<System.ArgumentException>());
        }

        [Test]
        public void WithTestContainer_should_set_test_container() {
            const string testContainer = "TestContainer";
            var runner = CreateRunner()
                .WithTestContainer(testContainer);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(testContainer));
        }

        [Test]
        public void WithTestContainer_null_should_use_all_containers() {
            var runner = CreateRunner()
                .WithTestContainer(null);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(AspRunner.AllTestContainers));
        }

        [Test]
        public void WithTestContainer_empty_should_use_all_containers() {
            var runner = CreateRunner()
                .WithTestContainer("");

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(AspRunner.AllTestContainers));
        }

        [Test]
        public void WithTestContainerAndCase_should_set_test_container_and_test_case() {
            const string testContainer = "TestContainer";
            const string testCase = "TestCase";
            var runner = CreateRunner()
                .WithTestContainerAndCase(testContainer, testCase);

            Assert.That(runner.GetField(TestContainerField),
                Is.EqualTo(testContainer));
            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(testCase));
        }

        [Test]
        public void WithTestContainerAndCase_with_null_test_case_should_use_all_test_cases() {
            var runner = CreateRunner()
                .WithTestContainerAndCase("TestContainer", null);

            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(AspRunner.AllTestCases));
        }

        [Test]
        public void WithTestContainerAndCase_with_empty_test_case_should_use_all_test_cases() {
            var runner = CreateRunner()
                .WithTestContainerAndCase("TestContainer", "");

            Assert.That(runner.GetField(TestCaseField),
                Is.EqualTo(AspRunner.AllTestCases));
        }

        private AspRunner CreateRunner() {
            return new AspRunner(_client, _resultParser, _selectorParser);
        }
    }
}
