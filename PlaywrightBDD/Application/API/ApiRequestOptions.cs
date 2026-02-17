using System.Collections.Generic;

namespace Application.API
{
    public class ApiRequestOptions
    {
        public Dictionary<string, string> Headers { get; } = new();
        public Dictionary<string, string> Query { get; } = new();

        public ApiRequestOptions WithHeader(string key, string value)
        {
            Headers[key] = value;
            return this;
        }

        public ApiRequestOptions WithQuery(string key, string value)
        {
            Query[key] = value;
            return this;
        }
    }
}