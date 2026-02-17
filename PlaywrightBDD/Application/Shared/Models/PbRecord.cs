using System.Collections.Generic;

namespace Application.Shared.Models
{
    public class PbRecord
    {
        public string? Id { get; set; }

        // Arbitrary fields for any collection
        public Dictionary<string, object?> Fields { get; } = new();

        public PbRecord Set(string key, object? value)
        {
            Fields[key] = value;
            return this;
        }

        public T? Get<T>(string key)
        {
            if (!Fields.TryGetValue(key, out var value) || value is null)
                return default;

            return (T?)value;
        }
    }
}