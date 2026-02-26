using System.Collections.Generic;

namespace Application.UI.Models
{
    public sealed class UsersRecord : IRecordData
    {
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string PasswordConfirm { get; init; } = default!;

        public IReadOnlyDictionary<string, string?> ToFields()
            => new Dictionary<string, string?>
            {
                ["email"] = Email, 
                ["Password"] = Password,
                ["Password confirm"] = PasswordConfirm
            };
    }
}