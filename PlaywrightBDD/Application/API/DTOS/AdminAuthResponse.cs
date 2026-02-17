using System.Text.Json.Serialization;

namespace Application.API.Dtos
{
    public class AdminAuthResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = "";
    }
}