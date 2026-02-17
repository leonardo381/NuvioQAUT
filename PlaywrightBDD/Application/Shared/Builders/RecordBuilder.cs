using Application.Shared.Models;

namespace Application.Shared.Builders
{
    public class RecordBuilder
    {
        private readonly PbRecord _record = new();

        public static RecordBuilder Create() => new();

        public RecordBuilder WithId(string id)
        {
            _record.Id = id;
            return this;
        }

        public RecordBuilder WithField(string key, object? value)
        {
            _record.Set(key, value);
            return this;
        }

        public PbRecord Build() => _record;
    }
}