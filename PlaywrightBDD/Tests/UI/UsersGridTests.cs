using Application.API;
using Application.Shared.Builders;
using Application.UI.Flows;
using Application.UI.Pages;
using Framework.Core;
using NUnit.Framework;

namespace Tests.UI
{
    public class UsersGridTests : BaseTest
    {
        [Test]
        [Category("UI")]
        [Category("Regression")]
        public async Task User_created_by_api_appears_in_users_grid()
        {
            var pb = new PocketBaseApi(Settings.BaseUrl);
            await pb.AdminLoginAsync(Settings.AdminUser, Settings.AdminPassword);

            // create user via API (depends on your PB users schema)
            var email = $"test{System.Guid.NewGuid():N}@test.com";
            var password = "Pass123!";

            var user = RecordBuilder.Create()
                .WithField("email", email)
                .WithField("password", password)
                .WithField("passwordConfirm", password)
                .Build();

            var created = await pb.CreateRecordAsync("users", user);

            // UI login + open users page
            var login = new LoginFlow(Page);
            await login.LoginToAdminAsync(Settings.AdminUser, Settings.AdminPassword);

            var usersPage = new UsersPage(Page);
            await usersPage.Menu.OpenUsersAsync();

            // Validate grid contains email (you can make a better Grid search later)
            var all = await usersPage.Grid.ReadAllCellsAsync();
            Assert.That(all.SelectMany(r => r).Any(cell => cell.Contains(email)), Is.True);

            // cleanup
            await pb.DeleteRecordAsync("users", created.Id!);
        }
    }
}