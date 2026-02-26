using System.Collections.Generic;

namespace Application.UI.Models
{
    /// <summary>
    /// Represents a generic UI record used by CollectionContext.
    /// Provides field label -> value mapping.
    /// </summary>
    public interface IRecordData
    {
        IReadOnlyDictionary<string, string?> ToFields();
    }
}