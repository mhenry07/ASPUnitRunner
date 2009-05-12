using System;
using System.Collections.Generic;
using System.Text;

namespace AspUnitRunner {
    public class Runner {
        private IProxy _proxy;

        public Runner()
            : this(new Proxy()) {
        }
        
        public Runner(IProxy proxy) {
            _proxy = proxy;
        }

        public Results Run() {
            return new Results();
        }
    }
}
