using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.API
{
    public class ApiClient
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

        public ApiClient(string baseUrl)
        {
            _http = new HttpClient { BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/") };
        }

        public void SetBearerToken(string token)
            => _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        public Task<ApiResponse> GetAsync(string path, ApiRequestOptions? opt = null)
            => SendAsync(HttpMethod.Get, path, null, opt);

        public Task<ApiResponse> DeleteAsync(string path, ApiRequestOptions? opt = null)
            => SendAsync(HttpMethod.Delete, path, null, opt);

        public Task<ApiResponse> PostAsync(string path, object? body, ApiRequestOptions? opt = null)
            => SendAsync(HttpMethod.Post, path, body, opt);

        public Task<ApiResponse> PutAsync(string path, object? body, ApiRequestOptions? opt = null)
            => SendAsync(HttpMethod.Put, path, body, opt);

        public Task<ApiResponse> PatchAsync(string path, object? body, ApiRequestOptions? opt = null)
            => SendAsync(new HttpMethod("PATCH"), path, body, opt);

        public Task<ApiResponse<T>> PatchAsync<T>(string path, object? body, ApiRequestOptions? opt = null)
            => SendAsync<T>(new HttpMethod("PATCH"), path, body, opt);

        public Task<ApiResponse<T>> GetAsync<T>(string path, ApiRequestOptions? opt = null)
            => SendAsync<T>(HttpMethod.Get, path, body: null, opt);

        public Task<ApiResponse<T>> PostAsync<T>(string path, object? body, ApiRequestOptions? opt = null)
            => SendAsync<T>(HttpMethod.Post, path, body, opt);

        private async Task<ApiResponse> SendAsync(HttpMethod method, string path, object? body, ApiRequestOptions? opt)
        {
            using var req = BuildRequest(method, path, body, opt);
            using var resp = await _http.SendAsync(req);
            var text = await resp.Content.ReadAsStringAsync();
            return new ApiResponse(resp.StatusCode, text, resp.Headers);
        }

        private async Task<ApiResponse<T>> SendAsync<T>(HttpMethod method, string path, object? body, ApiRequestOptions? opt)
        {
            using var req = BuildRequest(method, path, body, opt);
            using var resp = await _http.SendAsync(req);
            var text = await resp.Content.ReadAsStringAsync();

            T? data = default;
            if (!string.IsNullOrWhiteSpace(text))
            {
                try { data = JsonSerializer.Deserialize<T>(text, _json); }
                catch { /* keep data null */ }
            }

            return new ApiResponse<T>(resp.StatusCode, text, resp.Headers, data);
        }

        private HttpRequestMessage BuildRequest(HttpMethod method, string path, object? body, ApiRequestOptions? opt)
        {
            var finalPath = ApplyQuery(path, opt);
            var req = new HttpRequestMessage(method, finalPath);

            if (opt != null)
            {
                foreach (var kv in opt.Headers)
                    req.Headers.TryAddWithoutValidation(kv.Key, kv.Value);
            }

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body, _json);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return req;
        }

        private static string ApplyQuery(string path, ApiRequestOptions? opt)
        {
            if (opt == null || opt.Query.Count == 0) return path;

            var qs = string.Join("&", opt.Query.Select(kv =>
                $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

            return path.Contains("?") ? $"{path}&{qs}" : $"{path}?{qs}";
        }
    }
}