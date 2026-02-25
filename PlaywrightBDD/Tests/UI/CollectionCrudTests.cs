using System;
using System.Threading.Tasks;
using Application.UI;
using Application.UI.Pages;
using Framework.Assertions;
using Framework.Core;
using Microsoft.Playwright;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.UI
{
    
    public class CollectionCrudTests : BaseTest
    {
        
        private static string UniqueEmail()
            => $"ui_{Guid.NewGuid():N}@test.com";

        private const string DefaultPassword = "Pass12345!";

        // -----------------------------
        // CREATE
        // -----------------------------
        [Test]
        [Category(TestCategories.UI)]
        [Category(TestCategories.Regression)]
        public async Task Users_CreateUser_ShowsInGrid()
        {
            //await Page.PauseAsync();

            var nuvio = new Nuvio(Page, Executor);
            await nuvio.Login.AsAdminAsync();
            
            var usersPage = nuvio.Users;
            await usersPage.OpenAsync();

            var email = "test";
await Page.PauseAsync();
            await usersPage.CreateUserAsync(email, DefaultPassword);
            
            var row = usersPage.UsersGrid.RowByText(email);
            var count = await row.CountAsync();

            GenericAssert.IsTrue(count > 0,
                $"Expected to find a row for '{email}' after creation, but none was found.");
                
        }

        // -----------------------------
        // READ
        // -----------------------------
        [Test]
        [Category(TestCategories.UI)]
        [Category(TestCategories.Regression)]
        public async Task Users_ReadUser_AfterReload_StillVisibleInGrid()
        {
            var nuvio = new Nuvio(Page, Executor);

            await nuvio.Login.AsAdminAsync();

            var usersPage = nuvio.Users;
            await usersPage.OpenAsync();

            var email = UniqueEmail();
            await usersPage.CreateUserAsync(email, DefaultPassword);

            // Simulate a fresh "read" by reloading the page
            await Page.ReloadAsync();
            await usersPage.UsersGrid.WaitForLoadedAsync();

            var row = usersPage.UsersGrid.RowByText(email);
            var count = await row.CountAsync();

            GenericAssert.IsTrue(count > 0,
                $"Expected to find a row for '{email}' after reload, but none was found.");
        }

        // -----------------------------
        // UPDATE
        // -----------------------------
        [Test]
        [Category(TestCategories.UI)]
        [Category(TestCategories.Regression)]
        public async Task Users_UpdateUser_EmailIsChangedInGrid()
        {
            var nuvio = new Nuvio(Page, Executor);

            await nuvio.Login.AsAdminAsync();

            var usersPage = nuvio.Users;
            await usersPage.OpenAsync();

            var originalEmail = UniqueEmail();
            var newEmail = UniqueEmail();

            await usersPage.CreateUserAsync(originalEmail, DefaultPassword);

            // Open edit modal by clicking the row
            await usersPage.UsersGrid.RowByText(originalEmail).First.ClickAsync();

            // Update email via modal
            await usersPage.Modal.FillFieldAsync("Email", newEmail);
            await usersPage.Modal.ConfirmAsync();
            await usersPage.UsersGrid.WaitForLoadedAsync();

            // Assert old email is gone (or at least not present in grid)
            var oldRow = usersPage.UsersGrid.RowByText(originalEmail);
            var oldCount = await oldRow.CountAsync();

            // Assert new email is present
            var newRow = usersPage.UsersGrid.RowByText(newEmail);
            var newCount = await newRow.CountAsync();

            GenericAssert.IsTrue(newCount > 0,
                $"Expected to find updated row for '{newEmail}', but none was found.");

            // Not strictly required, but good safety check
            GenericAssert.IsTrue(oldCount == 0,
                $"Expected not to find old row for '{originalEmail}' after update, but it still exists.");
        }

        // -----------------------------
        // DELETE
        // -----------------------------
        [Test]
        [Category(TestCategories.UI)]
        [Category(TestCategories.Regression)]
        public async Task Users_DeleteUser_RowIsRemovedFromGrid()
        {
            var nuvio = new Nuvio(Page, Executor);

            await nuvio.Login.AsAdminAsync();

            var usersPage = nuvio.Users;
            await usersPage.OpenAsync();

            var email = UniqueEmail();
            await usersPage.CreateUserAsync(email, DefaultPassword);

            // Select the row (PocketBase usually shows toolbar actions for selected row)
            var row = usersPage.UsersGrid.RowByText(email).First;
            await row.ClickAsync();

            // For now, interact with the Delete button directly via Playwright.
            // If you later add Toolbar.ClickDeleteAsync(), swap this to use that.
            var deleteButton = Page.GetByRole(AriaRole.Button, new() { Name = "Delete" });
            await Executor.ClickAsync(deleteButton);

            // Confirm delete in the modal
            await usersPage.Modal.ConfirmAsync();
            await usersPage.UsersGrid.WaitForLoadedAsync();

            var remaining = usersPage.UsersGrid.RowByText(email);
            var count = await remaining.CountAsync();

            GenericAssert.IsTrue(count == 0,
                $"Expected user '{email}' to be deleted from grid, but the row still exists.");
        }
    }
}