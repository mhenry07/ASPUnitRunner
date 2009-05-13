using System;
using System.Collections.Generic;
using System.Text;

namespace AspUnitRunner {
    public interface IProxy {
        string GetTestResults(string uri, string postData);
    }
}
