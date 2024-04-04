using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace Mimeo.Blocks.Http
{
    public class ResponseEnvelope
    {
        // Request
        //
        public string Url { get; set; }
        public string RequestBody { get; set; }

        // Response
        //
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
        public byte[] BinaryData { get; set; }


        public ResponseEnvelope()
        {
            StatusCode = HttpStatusCode.Accepted;
        }

        public bool HasBadStatusCode =>
            !(this.StatusCode == HttpStatusCode.OK
              || this.StatusCode == HttpStatusCode.NoContent
              || this.StatusCode == HttpStatusCode.Created
              || this.StatusCode == HttpStatusCode.Accepted
              || this.StatusCode == HttpStatusCode.NotFound);



        public static ResponseEnvelope Make(HttpResponseMessage message)
        {
            var output = new ResponseEnvelope();
            output.Body = message.Content.ReadAsStringAsync().Result;
            output.StatusCode = message.StatusCode;
            output.Headers = new Dictionary<string, string>();

            foreach (var header in message.Headers)
            {
                output.Headers[header.Key] = string.Join("", header.Value);
            }

            return output;
        }
    }
}
