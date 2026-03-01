using System;
using System.Threading.Tasks;
using Application.UI.Context;
using Application.UI.Models;
using Application.UI.Pages;
using Application.UI.Components;
using Application.UI.Flows;
using Framework.Core;
using NUnit.Framework;

namespace Application.Tests.UI.Collections
{
    [TestFixture]
    public sealed class UsersCollectionCrudTests : BaseTest
    {
        private CollectionContext _collectionContext = default!;

        [SetUp]
        public async Task SetUpAsync()
        {
            // BaseTest should already start Playwright and create Page + Executor.

            // 1) Login as admin
            var login = new LoginFlow(Page, Executor);
            await login.AsAdminAsync();

            // 2) Build shell + page + context
            var shell = new AppShell(Page, Executor);
            var collectionPage = new CollectionPage(Page, Executor);

            _collectionContext = new CollectionContext(shell, collectionPage);
        }

        private static UsersRecord CreateRandomUser(
            string? prefix = null,
            string password = "Password123!")
        {
            var unique = Guid.NewGuid().ToString("N").Substring(0, 8);
            var emailPrefix = prefix ?? "ui-test";
            var email = $"{emailPrefix}.{unique}@example.com";

            return new UsersRecord
            {
                Email = email,
                Password = password,
                PasswordConfirm = password
            };
        }

        // --------------------------------------------------------
        // C - CREATE
        // --------------------------------------------------------
        [Test]
        public async Task CreateUser_ShouldAppearInGrid()
        {
            var record = CreateRandomUser("read");
            await _collectionContext.CreateAsync("users", record);

            await _collectionContext.AssertRowMatchesAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: record.Email,
                expected: record);
        }

        // --------------------------------------------------------
        // R - READ (AssertRowMatches)
        // --------------------------------------------------------
        [Test]
        public async Task ReadUser_ShouldMatchGridValues()
        {
            var record = CreateRandomUser("read");
            await _collectionContext.CreateAsync("users", record);

            await _collectionContext.AssertRowMatchesAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: record.Email,
                expected: record);
        }

        // --------------------------------------------------------
        // U - UPDATE
        // --------------------------------------------------------
        [Test]
        public async Task UpdateUser_ShouldReflectNewValuesInGrid()
        {
            var original = CreateRandomUser("update");
            await _collectionContext.CreateAsync("users", original);

            var updated = new UsersRecord
            {
                Email = "UP" + original.Email
            };

            await _collectionContext.UpdateAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: original.Email,
                data: updated);

            await _collectionContext.AssertRowMatchesAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: original.Email,
                expected: updated);
        }

        // --------------------------------------------------------
        // D - DELETE
        // --------------------------------------------------------
        [Test]
        public async Task DeleteUser_ShouldRemoveRowFromGrid()
        {
            var record = CreateRandomUser("delete");
            await _collectionContext.CreateAsync("users", record);

            await _collectionContext.DeleteAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: record.Email);

            await _collectionContext.AssertRowNotExistsAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: record.Email);
        }
    }
}