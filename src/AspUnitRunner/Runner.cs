using System;
using System.Collections.Generic;
using System.Text;

namespace AspUnitRunner {
    public class Runner {
        private IProxy _proxy;
        private string _baseUri;

        public Runner(string baseUri)
            : this(baseUri, new Proxy()) {
        }
        
        public Runner(string baseUri, IProxy proxy) {
            _baseUri = baseUri;
            _proxy = proxy;
        }

        public Results Run() {
            string htmlResults = _proxy.GetTestResults(_baseUri + "?UnitRunner=results", "");
            return new Results(htmlResults);
        }
    }
}
