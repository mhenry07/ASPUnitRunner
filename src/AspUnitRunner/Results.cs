using System;
using System.Collections.Generic;
using System.Text;

namespace AspUnitRunner {
    public class Results {
        public int Errors { get; private set; }
        public int Failures { get; private set; }

        public Results() {
            Errors = 1;
            Failures = 1;
        }
    }
}
