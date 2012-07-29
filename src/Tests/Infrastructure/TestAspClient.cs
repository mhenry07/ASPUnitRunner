using System.Collections.Specialized;
using System.Net;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner.Core;
using AspUnitRunner.Infrastructure;

namespace AspUnitRunner.Tests.Infrastructure {
    [TestFixture]
    public class TestAspClient {
        [Test]
        public void GetTestResults_should_upload_values_and_return_expected_response() {
            // Arrange
            const string address = "http://path/to/test-runner?key=value";
            var postValues = new NameValueCollection() {
                { "key1", "value 1" },
                { "key2", "value 2" }
            };
            const string postData = "key1=value+1&key2=value+2";
            var credentials = MockRepository.GenerateStub<ICredentials>();
            const string expectedResponse = "response";

            var factory = MockRepository.GenerateStub<IWebClientFactory>();
            var webClient = MockRepository.GenerateStub<WebClient>();
            webClient.Headers = new WebHeaderCollection();
            factory.Stub(f => f.Create()).Return(webClient);
            webClient.Stub(c => c.UploadString(address, postData))
                .Return(expectedResponse);

            // Act
            var aspClient = new AspClient(factory);
            var response = aspClient.GetTestResults(address, postValues, credentials);

            // Assert
            Assert.That(webClient.Credentials, Is.EqualTo(credentials));
            Assert.That(webClient.Headers[HttpRequestHeader.ContentType],
                Is.EqualTo("application/x-www-form-urlencoded"));
            Assert.That(response, Is.EqualTo(expectedResponse));
        }
    }
}
