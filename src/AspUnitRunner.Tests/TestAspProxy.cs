using System.IO;
using System.Net;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace AspUnitRunner.Tests {
    [TestFixture]
    public class TestAspProxy {
        WebRequest _request;
        WebResponse _response;
        Stream _requestStream;
        Stream _responseStream;

        [SetUp]
        public void Setup() {
            _request = MockRepository.GenerateStub<WebRequest>();
            _response = MockRepository.GenerateStub<WebResponse>();
            _requestStream = MockRepository.GenerateMock<Stream>();
            _responseStream = new MemoryStream();
            _request.Stub(x => x.GetRequestStream())
                .Return(_requestStream);
            _request.Stub(x => x.GetResponse())
                .Return(_response);
            _response.Stub(x => x.GetResponseStream())
                .Return(_responseStream);

            WebRequest.RegisterPrefix("fake:", new FakeWebRequestCreate(_request));
        }

        [TearDown]
        public void TearDown() {
            _responseStream.Dispose();
        }

        [Test]
        public void GetTestResults_should_post_request_and_return_response() {
            var encoding = new ASCIIEncoding();
            var postData = "postdata";
            var postBytes = encoding.GetBytes(postData);
            var expectedResponse = "response";
            var responseBytes = encoding.GetBytes(expectedResponse);
            _responseStream.Write(responseBytes, 0, responseBytes.Length);
            _responseStream.Position = 0;

            var proxy = new AspProxy();
            var results = proxy.GetTestResults("fake://host", postData, null);

            Assert.That(_request.Method, Is.EqualTo(WebRequestMethods.Http.Post));
            Assert.That(_request.Credentials, Is.Null);
            Assert.That(_request.ContentType, Is.EqualTo("application/x-www-form-urlencoded"));
            Assert.That(_request.ContentLength, Is.EqualTo(postBytes.Length));
            _requestStream.AssertWasCalled(x => x.Write(postBytes, 0, postBytes.Length));
            Assert.That(results, Is.EqualTo(expectedResponse));
        }
    }
}
