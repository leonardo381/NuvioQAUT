using System;
using System.Net;

namespace Application.API.Exceptions
{
    /// <summary>
    /// Represents a failed API call to the SUT (PocketBase).
    /// Includes request/response context for debugging.
    /// </summary>
    public sealed class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Method { get; }
        public string Url { get; }
        public string? RequestBody { get; }
        public string ResponseBody { get; }

        public ApiException(
            string message,
            HttpStatusCode statusCode,
            string method,
            string url,
            string? requestBody,
            string responseBody,
            Exception? inner = null)
            : base(message, inner)
        {
            StatusCode = statusCode;
            Method = method;
            Url = url;
            RequestBody = requestBody;
            ResponseBody = responseBody;
        }

        public override string ToString()
        {
            return
$@"{Message}
Status: {(int)StatusCode} {StatusCode}
Request: {Method} {Url}
RequestBody:
{RequestBody ?? "(none)"}
ResponseBody:
{ResponseBody}";
        }
    }
}