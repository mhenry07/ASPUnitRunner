using System;
using System.Net;
using System.Net.Mime;
using System.Text;
using AspUnitRunner.Core;

namespace AspUnitRunner.Infrastructure {
    internal class ResponseDecoder : IResponseDecoder {
        public string DecodeResponse(WebClient webClient, byte[] responseBytes) {
            var charset = GetCharset(webClient.ResponseHeaders);
            var encoding = GetEncoding(charset, webClient.Encoding);
            return encoding.GetString(responseBytes);
        }

        internal static string GetCharset(WebHeaderCollection responseHeaders) {
            var contentType = responseHeaders[HttpResponseHeader.ContentType];
            if (string.IsNullOrEmpty(contentType))
                return null;

            try {
                return new ContentType(contentType).CharSet;
            } catch (FormatException) {
                return null;
            }
        }

        internal static Encoding GetEncoding(string charset, Encoding defaultEncoding) {
            if (string.IsNullOrEmpty(charset))
                return defaultEncoding;

            try {
                return Encoding.GetEncoding(charset);
            } catch (ArgumentException) {
                return defaultEncoding;
            }
        }
    }
}
