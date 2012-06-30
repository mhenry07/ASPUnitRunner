using System;
using System.Net;

namespace AspUnitRunner.Tests {
    public class FakeWebRequestCreate : IWebRequestCreate {
        WebRequest _request;

        public FakeWebRequestCreate(WebRequest request) {
            _request = request;
        }

        public WebRequest Create(Uri uri) {
            return _request;
        }
    }
}
