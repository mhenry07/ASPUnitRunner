using System.Net;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using AspUnitRunner.Infrastructure;

namespace AspUnitRunner.Tests.Unit.Infrastructure {
    [TestFixture]
    public class TestResponseDecoder {
        // from http://www.columbia.edu/~fdc/utf8/
        private const string InternationalCurrencySymbols =
            "¥ · £ · € · $ · ¢ · ₡ · ₢ · ₣ · ₤ · ₥ · ₦ · ₧ · ₨ · ₩ · ₪ · ₫ · ₭ · ₮ · ₯ · ₹";

        private WebClient _webClient;

        [SetUp]
        public void SetUp() {
            _webClient = MockRepository.GenerateStub<WebClient>();
            _webClient.Stub(c => c.ResponseHeaders)
                .Return(new WebHeaderCollection());
        }

        [Test]
        public void DecodeResponse_ascii_should_return_expected_response() {
            _webClient.Encoding = Encoding.ASCII;
            var expectedResponse = "response";
            var responseBytes = Encoding.ASCII.GetBytes(expectedResponse);

            var decoder = new ResponseDecoder();
            var response = decoder.DecodeResponse(_webClient, responseBytes);

            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void DecodeResponse_with_utf8_default_should_return_expected_response() {
            _webClient.Encoding = Encoding.UTF8;
            var expectedResponse = InternationalCurrencySymbols;
            var responseBytes = Encoding.UTF8.GetBytes(expectedResponse);

            var decoder = new ResponseDecoder();
            var response = decoder.DecodeResponse(_webClient, responseBytes);

            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void DecodeResponse_with_utf8_response_header_should_return_expected_response() {
            _webClient.Encoding = Encoding.ASCII;
            _webClient.ResponseHeaders.Add(HttpResponseHeader.ContentType, "text/html; charset=UTF-8");
            var expectedResponse = InternationalCurrencySymbols;
            var responseBytes = Encoding.UTF8.GetBytes(expectedResponse);

            var decoder = new ResponseDecoder();
            var response = decoder.DecodeResponse(_webClient, responseBytes);

            Assert.That(response, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void GetCharset_should_return_UTF_8() {
            var headers = new WebHeaderCollection {
                { HttpResponseHeader.ContentType, "text/html; charset=UTF-8" }
            };

            var charset = ResponseDecoder.GetCharset(headers);
            Assert.That(charset, Is.EqualTo("UTF-8"));
        }

        [Test]
        public void GetCharset_should_return_utf_8() {
            var headers = new WebHeaderCollection {
                { HttpResponseHeader.ContentType, "text/html; charset=utf-8" }
            };

            var charset = ResponseDecoder.GetCharset(headers);
            Assert.That(charset, Is.EqualTo("utf-8"));
        }

        [Test]
        public void GetCharset_generic_should_return_generic() {
            var headers = new WebHeaderCollection {
                { HttpResponseHeader.ContentType, "text/html; charset=generic" }
            };

            var charset = ResponseDecoder.GetCharset(headers);
            Assert.That(charset, Is.EqualTo("generic"));
        }

        [Test]
        public void GetCharset_no_charset_should_return_null() {
            var headers = new WebHeaderCollection {
                { HttpResponseHeader.ContentType, "text/html" }
            };

            var charset = ResponseDecoder.GetCharset(headers);
            Assert.That(charset, Is.Null);
        }

        [Test]
        public void GetCharset_invalid_should_return_null() {
            var headers = new WebHeaderCollection {
                { HttpResponseHeader.ContentType, "<invalid>" }
            };

            var charset = ResponseDecoder.GetCharset(headers);
            Assert.That(charset, Is.Null);
        }

        [Test]
        public void GetCharset_no_content_type_should_return_null() {
            var headers = new WebHeaderCollection {
                { HttpResponseHeader.ContentType, null }
            };

            var charset = ResponseDecoder.GetCharset(headers);
            Assert.That(charset, Is.Null);
        }

        [Test]
        public void GetEncoding_UTF_8_should_return_UTF8Encoding() {
            var defaultEncoding = Encoding.GetEncoding("Windows-1252");

            var encoding = ResponseDecoder.GetEncoding("UTF-8", defaultEncoding);
            Assert.That(encoding, Is.InstanceOf<UTF8Encoding>());
        }

        [Test]
        public void GetEncoding_utf_8_should_return_UTF8Encoding() {
            var defaultEncoding = Encoding.GetEncoding("Windows-1252");

            var encoding = ResponseDecoder.GetEncoding("utf-8", defaultEncoding);
            Assert.That(encoding, Is.InstanceOf<UTF8Encoding>());
        }

        [Test]
        public void GetEncoding_invalid_should_return_default_encoding() {
            var defaultEncoding = Encoding.GetEncoding("Windows-1252");

            var encoding = ResponseDecoder.GetEncoding("invalid", defaultEncoding);
            Assert.That(encoding, Is.EqualTo(defaultEncoding));
        }

        [Test]
        public void GetEncoding_null_should_return_default_encoding() {
            var defaultEncoding = Encoding.GetEncoding("Windows-1252");

            var encoding = ResponseDecoder.GetEncoding(null, defaultEncoding);
            Assert.That(encoding, Is.EqualTo(defaultEncoding));
        }
    }
}
