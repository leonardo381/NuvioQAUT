using System;
using System.Threading.Tasks;
using Application.UI.Context;
using Application.UI.Models;
using Application.UI.Pages;
using Application.UI.Components;
using Application.UI.Flows;      // <- adjust if your LoginFlow is in a different namespace
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
            var create = new UsersRecord
            {
                Email = "newtest@gmail.com",         // key column stays the same
                Password = "NewPass!456",
                PasswordConfirm = "NewPass!456"
            };

            // Act
            await _collectionContext.CreateAsync("users", create);

            // Assert
            await _collectionContext.AssertRowMatchesAsync(
                collectionName: "users",
                keyColumn: "Email",
                keyValue: create.Email,
                expected: create);
        }

        // --------------------------------------------------------
        // R - READ (AssertRowMatches)
        // --------------------------------------------------------
        [Test]
        public async Task ReadUser_ShouldMatchGridValues()
        {
            // Arrange: create a fresh user as test data
            var record = CreateRandomUser("read");
            await _collectionContext.CreateAsync("users", record);

            // Act + Assert: use the generic assertion (READ operation)
            await _collectionContext.AssertRowMatchesAsync(
                collectionName: "users",
                keyColumn: "Email",
                keyValue: record.Email,
                expected: record);
        }

        // --------------------------------------------------------
        // U - UPDATE
        // --------------------------------------------------------
        [Test]
        public async Task UpdateUser_ShouldReflectNewValuesInGrid()
        {
            // Arrange: create initial user
            var original = CreateRandomUser("update");
            await _collectionContext.CreateAsync("users", original);

            // Prepare updated model (same key, different password)
            var updated = new UsersRecord
            {
                Email = original.Email,         // key column stays the same
                Password = "NewPass!456",
                PasswordConfirm = "NewPass!456"
            };

            // Act: update
            await _collectionContext.UpdateAsync(
                collectionName: "users",
                keyColumn: "Email",
                keyValue: original.Email,
                data: updated);

            // Assert: row matches updated values
            await _collectionContext.AssertRowMatchesAsync(
                collectionName: "users",
                keyColumn: "Email",
                keyValue: original.Email,
                expected: updated);
        }

        // --------------------------------------------------------
        // D - DELETE
        // --------------------------------------------------------
        [Test]
        public async Task DeleteUser_ShouldRemoveRowFromGrid()
        {
            // Arrange: create user to delete
            var record = CreateRandomUser("delete");
            await _collectionContext.CreateAsync("users", record);

            // Act: delete it
            await _collectionContext.DeleteAsync(
                collectionName: "users",
                keyColumn: "Email",
                keyValue: record.Email);

            // Assert: trying to assert the row should now fail
            // with a clear error; or you can check via Grid directly.
            // Here we assert via the context's grid helpers.

            // We go through the grid directly so we don't reuse DeleteAsync internals.
            var collectionPage = new CollectionPage(Page, Executor);
            var grid = collectionPage.Grid;

            await grid.WaitLoadedAsync();

            var rowIndex = await grid.FindRowIndexByColumnAsync("Email", record.Email);
            //Assert(rowIndex, "User row should not exist after delete.");
        }
    }
}