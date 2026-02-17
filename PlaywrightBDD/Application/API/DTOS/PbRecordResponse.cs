using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.API.Dtos
{
    public class PbRecordResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";

        // capture remaining fields
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Fields { get; set; } = new();
    }
}