using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace Application.UI.PageTest
{
    public sealed class PageTestLoginPage
    {
        private readonly IPage _page;

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
    }
}
