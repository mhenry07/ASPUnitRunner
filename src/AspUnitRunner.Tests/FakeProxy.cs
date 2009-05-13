using System;
using System.Collections.Generic;
using System.Text;

namespace AspUnitRunner.Tests {
    class FakeProxy : IProxy {
        public string HtmlResults { get; set; }
        public string Url { get; set; }
        public string PostData { get; set; }

        #region IProxy Members

        public string GetTestResults(string url, string postData) {
            Url = url;
            PostData = postData;
            return HtmlResults;
        }

        #endregion
    }
}
