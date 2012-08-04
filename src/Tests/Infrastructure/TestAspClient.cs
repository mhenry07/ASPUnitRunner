using System.Collections.Specialized;
using System.Net;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner.Core;
using AspUnitRunner.Infrastructure;

namespace AspUnitRunner.Tests.Infrastructure {
    [TestFixture]
    public class TestAspClient {
        private IWebClientFactory _factory;
        private IResponseDecoder _responseDecoder;
        private WebClient _webClient;

        [SetUp]
        public void SetUp() {
            _factory = MockRepository.GenerateStub<IWebClientFactory>();
            _responseDecoder = MockRepository.GenerateStub<IResponseDecoder>();
            _webClient = MockRepository.GenerateStub<WebClient>();
            _factory.Stub(f => f.Create()).Return(_webClient);
            _webClient.Headers = new WebHeaderCollection();
        }

        [Test]
        public void PostRequest_should_upload_values_and_return_expected_response() {
            // Arrange
            const string address = "http://path/to/test-runner?key=value";
            var postValues = new NameValueCollection() {
                { "key1", "value 1" },
                { "key2", "value 2" }
            };
            const string expectedResponse = "response";
            var responseBytes = Encoding.Default.GetBytes(expectedResponse);

            _webClient.Stub(c => c.UploadValues(address, postValues))
                .Return(responseBytes);
            _responseDecoder.Stub(d => d.DecodeResponse(_webClient, responseBytes))
                .Return(expectedResponse);

            // Act
            var aspClient = new AspClient(_factory, _responseDecoder);
            var response = aspClient.PostRequest(address, postValues);

            // Assert
            Assert.That(_webClient.Credentials, Is.Null);
            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void PostRequest_with_credentials_should_set_web_client_credentials() {
            var credentials = new NetworkCredential("username", "password");

            var aspClient = new AspClient(_factory, _responseDecoder);
            aspClient.Credentials = credentials;
            var response = aspClient.PostRequest("", null);

            Assert.That(_webClient.Credentials, Is.EqualTo(credentials));
        }

        [Test]
        public void PostRequest_with_encoding_should_set_web_client_encoding() {
            var encoding = Encoding.UTF8;

            var aspClient = new AspClient(_factory, _responseDecoder);
            aspClient.Encoding = encoding;
            var response = aspClient.PostRequest("", null);

            Assert.That(_webClient.Encoding, Is.EqualTo(encoding));
        }

        [Test]
        public void PostRequest_with_null_encoding_should_keep_web_client_encoding() {
            var defaultEncoding = Encoding.GetEncoding("windows-1252");
            _webClient.Encoding = defaultEncoding;

            var aspClient = new AspClient(_factory, _responseDecoder);
            aspClient.Encoding = null;
            var response = aspClient.PostRequest("", null);

            Assert.That(_webClient.Encoding, Is.EqualTo(defaultEncoding));
        }
    }
}
