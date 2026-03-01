using System;
using System.Threading.Tasks;
using Application.UI.Context;
using Application.UI.Models;
using Application.UI.Pages;
using Application.UI.Components;
using Application.UI.Flows;
using Framework.Core;
using Tests.Helpers;
using NUnit.Framework;

namespace Application.Tests.UI.Collections
{
    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)] 
    [Parallelizable(ParallelScope.All)]

    [Category(TestCategories.UI)]
    [Category(TestCategories.Regression)]
    public sealed class UsersCollectionCrudTests : BaseTest
    {
        private CollectionContext _collectionContext = default!;

        [SetUp]
        public async Task SetUpAsync()
        {
            var login = new LoginFlow(Page, Executor);
            await login.AsAdminAsync();

            var shell = new AppShell(Page, Executor);
            var collectionPage = new CollectionPage(Page, Executor);

            _collectionContext = new CollectionContext(shell, collectionPage);
        }

        // --------------------------------------------------------
        // C - CREATE
        // --------------------------------------------------------
        [Test]
        public async Task CreateUser_ShouldAppearInGrid()
        {
            var record = TestDataFactory.NewUser("create");

            await _collectionContext.CreateAsync(
                collectionName: "users",
                data: record);

            await _collectionContext.AssertRowMatchesAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: record.Email,
                expected: record);
        }

        // --------------------------------------------------------
        // R - READ
        // --------------------------------------------------------
        [Test]
        public async Task ReadUser_ShouldMatchGridValues()
        {
            var record = TestDataFactory.NewUser("read");

            await _collectionContext.CreateAsync(
                collectionName: "users",
                data: record);

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
            var record = TestDataFactory.NewUser("update-orig");
            await _collectionContext.CreateAsync("users", record);

            var updated = TestDataFactory.UpdatedUserFrom(record, "update-new");

            await _collectionContext.UpdateAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: record.Email,
                data: updated);

            await _collectionContext.AssertRowMatchesAsync(
                collectionName: "users",
                keyColumn: "email",
                keyValue: updated.Email,   // note: key changed
                expected: updated);
        }

        // --------------------------------------------------------
        // D - DELETE
        // --------------------------------------------------------
        [Test]
        public async Task DeleteUser_ShouldRemoveRowFromGrid()
        {
            var record = TestDataFactory.NewUser("delete");

            await _collectionContext.CreateAsync(
                collectionName: "users",
                data: record);

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