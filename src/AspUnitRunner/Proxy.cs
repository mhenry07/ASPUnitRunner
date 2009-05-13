using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace AspUnitRunner {
    // Note: this class isn't under test (it depends on WebRequest and WebResponse which are hard to fake).
    class Proxy : IProxy {
        public string GetTestResults(string uri, string postData, ICredentials credentials) {
            WebRequest request = WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded";
            SetPostData(request, postData);

            return GetResponse(request);
        }

        private void SetPostData(WebRequest request, string postData) {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postBytes = encoding.GetBytes(postData);
            request.ContentLength = postBytes.Length;
            using (Stream requestStream = request.GetRequestStream()) {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }
        }

        private string GetResponse(WebRequest request) {
            using (WebResponse response = request.GetResponse()) {
                using (StreamReader responseStream = new StreamReader(response.GetResponseStream())) {
                    return responseStream.ReadToEnd();
                }
            }
        }
    }
}
