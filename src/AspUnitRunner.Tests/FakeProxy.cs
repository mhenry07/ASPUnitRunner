using System;
using System.Collections.Generic;
using System.Text;

namespace AspUnitRunner.Tests {
    class FakeProxy : IProxy {
        public string HtmlResults { get; set; }

        #region IProxy Members

        public string GetTestResults(string url, string postData) {
            return HtmlResults;
        }

        #endregion
    }
}
