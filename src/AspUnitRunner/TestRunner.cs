using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace AspUnitRunner {
    public class TestRunner {
        private const string BaseQueryString = "?UnitRunner=results";
        private const string DefaultTestCases = "All Test Cases";
        private const string RunValue = "Run Tests";

        private string _baseUri;
        private ICredentials _credentials;

        // Note: For default credentials to work the test directory must have Integrated Windows Authentication enabled.
        public TestRunner(string baseUri) : this(baseUri, CredentialCache.DefaultCredentials) { }
        
        public TestRunner(string baseUri, ICredentials credentials) {
            _baseUri = baseUri;
            _credentials = credentials;
        }

        public TestResults Run(string testContainer) {
            WebRequest request = WebRequest.Create(_baseUri + BaseQueryString);
            request.Method = "POST";
            request.Credentials = _credentials;

            string postData = String.Format("cboTestContainers={0}&cboTestCases={1}&cmdRun={2}", 
                HttpUtility.UrlEncode(testContainer), HttpUtility.UrlEncode(DefaultTestCases), HttpUtility.UrlEncode(RunValue));
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postBytes = encoding.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            using (Stream requestStream = request.GetRequestStream()) {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }

            using (WebResponse response = request.GetResponse()) {
                using (StreamReader responseStream = new StreamReader(response.GetResponseStream())) {
                    return new TestResults(responseStream.ReadToEnd());
                }
            }
        }
    }
}
