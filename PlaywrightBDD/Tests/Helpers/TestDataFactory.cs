using System;
using Application.UI.Models;   // where UsersRecord lives

namespace Tests.Helpers
{
    /// <summary>
    /// Central factory for building test data models used by UI/API tests
    /// </summary>
    public static class TestDataFactory
    {
        public static UsersRecord NewUser(
            string? scenarioPrefix = null,
            string password = "Password123!")
        {
            var unique = Guid.NewGuid().ToString("N")[..8];
            var label = string.IsNullOrWhiteSpace(scenarioPrefix)
                ? "ui"
                : $"ui-{scenarioPrefix}";

            var email = $"{label}.{unique}@example.com";

            return new UsersRecord
            {
                Email = email,
                Password = password,
                PasswordConfirm = password
            };
        }

        public static UsersRecord UpdatedUserFrom(
            UsersRecord original,
            string? scenarioPrefix = "update")
        {
            var unique = Guid.NewGuid().ToString("N")[..8];
            var label = string.IsNullOrWhiteSpace(scenarioPrefix)
                ? "ui-update"
                : $"ui-{scenarioPrefix}";

            var newEmail = $"{label}.{unique}@example.com";

            return new UsersRecord
            {
                Email = newEmail
            };
        }
    }
}