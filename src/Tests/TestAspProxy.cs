using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestAspProxy {
        private IWebRequestFactory _factory;
        private WebRequest _request;
        private WebResponse _response;
        private Stream _requestStream;

        [SetUp]
        public void SetUp() {
            _factory = MockRepository.GenerateStub<IWebRequestFactory>();
            _request = MockRepository.GenerateStub<WebRequest>();
            _response = MockRepository.GenerateStub<WebResponse>();
            _requestStream = MockRepository.GenerateMock<Stream>();
            _factory.Stub(factory => factory.Create(Arg<string>.Is.Anything))
                .Return(_request);
            _request.Stub(request => request.GetRequestStream())
                .Return(_requestStream);
            _request.Stub(request => request.GetResponse())
                .Return(_response);
        }

        [Test]
        public void GetTestResults_should_post_request_and_return_expected_response() {
            string results;

            var postValues = new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("key1", "value 1"),
                new KeyValuePair<string, string>("key2", "value 2"),
            };
            const string postData = "key1=value+1&key2=value+2";
            var postBytes = Encoding.ASCII.GetBytes(postData);
            const string expectedResponse = "response";
            using (var responseStream = SetupResponseStream(expectedResponse)) {

                var proxy = new AspProxy(_factory);
                results = proxy.GetTestResults("fake://host", postValues, null);
            }

            Assert.That(_request.Method, Is.EqualTo(WebRequestMethods.Http.Post));
            Assert.That(_request.ContentType, Is.EqualTo("application/x-www-form-urlencoded"));
            Assert.That(_request.Credentials, Is.Null);
            Assert.That(_request.ContentLength, Is.EqualTo(postBytes.Length));
            _requestStream.AssertWasCalled(stream => stream.Write(postBytes, 0, postBytes.Length));
            Assert.That(results, Is.EqualTo(expectedResponse));
        }

        private Stream SetupResponseStream(string expectedResponse) {
            var bytes = Encoding.ASCII.GetBytes(expectedResponse);
            var stream = new MemoryStream(bytes);
            _response.Stub(response => response.GetResponseStream())
                .Return(stream);
            return stream;
        }
    }
}
