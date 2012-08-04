using System.Net;

namespace AspUnitRunner.Core {
    internal interface IResponseDecoder {
        string DecodeResponse(WebClient webClient, byte[] responseBytes);
    }
}
