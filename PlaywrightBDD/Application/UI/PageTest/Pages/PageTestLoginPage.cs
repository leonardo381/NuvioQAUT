using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.UI.PageTest.Pages
{
    public sealed class PageTestLoginPage
    {
        private readonly IPage _page;

        public PageTestLoginPage(IPage page)
        {
            _page = page;
        }

        private ILocator IdentityInput =>
            _page.Locator("input[name='identity'], input[name='email'], input[type='email']").First;

        private ILocator PasswordInput =>
            _page.Locator("input[name='password'], input[type='password']").First;

        private ILocator SubmitButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Sign in" })
                .Or(_page.GetByRole(AriaRole.Button, new() { Name = "Login" }))
                .First;

        public Task<IResponse?> GotoAsync(string baseUrl)
            => _page.GotoAsync($"{baseUrl.TrimEnd('/')}/_/#/login");

        public async Task AssertLoadedAsync()
        {
            await Assertions.Expect(IdentityInput).ToBeVisibleAsync();
            await Assertions.Expect(PasswordInput).ToBeVisibleAsync();
            await Assertions.Expect(SubmitButton).ToBeVisibleAsync();
        }

        public async Task FillCredentialsAsync(string usernameOrEmail, string password)
        {
            await IdentityInput.FillAsync(usernameOrEmail);
            await PasswordInput.FillAsync(password);
        }

        public Task SubmitAsync()
            => SubmitButton.ClickAsync();

        public async Task LoginAsync(string baseUrl, string usernameOrEmail, string password)
        {
            await GotoAsync(baseUrl);
            await AssertLoadedAsync();
            await FillCredentialsAsync(usernameOrEmail, password);
            await SubmitAsync();
        }

        public async Task AssertAuthenticatedAsync()
        {
            // Current clean-path signal: successful auth leaves the login route and removes the login form.
            await Assertions.Expect(_page).ToHaveURLAsync(new Regex(".*/_/#/(?!login(?:$|[/?#])).*"));
            await Assertions.Expect(IdentityInput).ToBeHiddenAsync();
            await Assertions.Expect(PasswordInput).ToBeHiddenAsync();
        }
    }
}
