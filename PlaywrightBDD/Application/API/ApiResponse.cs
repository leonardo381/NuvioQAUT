using System.Net;
using System.Net.Http.Headers;

namespace Application.API
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; }
        public string Body { get; }
        public HttpResponseHeaders Headers { get; }
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;

        public ApiResponse(HttpStatusCode statusCode, string body, HttpResponseHeaders headers)
        {
            StatusCode = statusCode;
            Body = body;
            Headers = headers;
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; }

        public ApiResponse(HttpStatusCode statusCode, string body, HttpResponseHeaders headers, T? data)
            : base(statusCode, body, headers)
        {
            Data = data;
        }
    }
}