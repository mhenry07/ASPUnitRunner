using System;
using System.Collections.Generic;
using System.Text;

namespace AspUnitRunner {
    public class Runner {
        private const string BaseQueryString = "?UnitRunner=results";

        private IProxy _proxy;
        private string _baseUri;

        public Runner(string baseUri)
            : this(baseUri, new Proxy()) {
        }
        
        public Runner(string baseUri, IProxy proxy) {
            _baseUri = baseUri;
            _proxy = proxy;
        }

        public Results Run(string testContainer) {
            string htmlResults = _proxy.GetTestResults(GetUri(), "");
            return new Results(htmlResults);
        }

        private string GetUri() {
            return _baseUri + BaseQueryString;
        }
    }
}
