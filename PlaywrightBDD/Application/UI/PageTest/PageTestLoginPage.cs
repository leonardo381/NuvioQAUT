using Microsoft.Playwright;
using System.Text.RegularExpressions;
using static Microsoft.Playwright.Assertions;

namespace Application.UI.PageTest
{
    public sealed class PageTestLoginPage
    {
        private readonly IPage _page;

        // PocketBase admin login inputs expose stable name/type attributes; labels vary between UI versions.
        private ILocator IdentityInput =>
            _page.Locator("input[name='identity'], input[name='email'], input[type='email']");

        private ILocator PasswordInput =>
            _page.Locator("input[name='password'], input[type='password']");

        private ILocator SubmitButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Sign in" })
                .Or(_page.GetByRole(AriaRole.Button, new() { Name = "Login" }))
                .Or(_page.GetByRole(AriaRole.Button, new() { Name = "Log in" }))
                .Or(_page.Locator("button[type='submit'], input[type='submit']"));

        public PageTestLoginPage(IPage page)
        {
            _page = page;
        }

        public async Task GotoAsync(string baseUrl)
        {
            await _page.GotoAsync($"{baseUrl.TrimEnd('/')}/_/#/login");
        }

        public async Task AssertLoadedAsync()
        {
            await Expect(IdentityInput).ToBeVisibleAsync();
            await Expect(PasswordInput).ToBeVisibleAsync();
            await Expect(SubmitButton).ToBeVisibleAsync();
        }

        public async Task FillCredentialsAsync(string usernameOrEmail, string password)
        {
            await Expect(IdentityInput).ToBeVisibleAsync();
            await IdentityInput.FillAsync(usernameOrEmail);

            await Expect(PasswordInput).ToBeVisibleAsync();
            await PasswordInput.FillAsync(password);
        }

        public async Task SubmitAsync()
        {
            await SubmitButton.ClickAsync();
        }

        public async Task LoginAsync(string baseUrl, string usernameOrEmail, string password)
        {
            await GotoAsync(baseUrl);
            await AssertLoadedAsync();
            await FillCredentialsAsync(usernameOrEmail, password);
            await SubmitAsync();
        }

        public async Task AssertAuthenticatedAsync()
        {
            await Expect(_page).Not.ToHaveURLAsync(new Regex(@"/_/#/login", RegexOptions.IgnoreCase));
            await Expect(IdentityInput).ToBeHiddenAsync();
            await Expect(PasswordInput).ToBeHiddenAsync();
        }
    }
}
