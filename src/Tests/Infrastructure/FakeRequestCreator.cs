using System;
using System.Net;

namespace AspUnitRunner.Tests.Infrastructure {
    // this is a singleton since WebRequest.RegisterPrefix only registers the first instance
    internal sealed class FakeRequestCreator : IWebRequestCreate {
        private static readonly FakeRequestCreator _instance = new FakeRequestCreator();

        private FakeRequestCreator() {
        }

        public static FakeRequestCreator Instance {
            get {
                return _instance;
            }
        }
        public WebRequest Request { get; set; }

        public WebRequest Create(Uri uri) {
            return Request;
        }
    }
}
