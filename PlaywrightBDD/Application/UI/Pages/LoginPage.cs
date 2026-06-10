using Framework.Engine;
using Microsoft.Playwright;
using System.Threading.Tasks;
using static Microsoft.Playwright.Assertions;

namespace Application.UI.Pages
{
    public sealed class LoginPage
    {
        private readonly IPage _page;
        private readonly ExecutionSettings _settings;

        private ILocator IdentityInput =>
            _page.Locator("input[name='identity'], input[name='email'], input[type='email']");

        private ILocator PasswordInput =>
            _page.Locator("input[name='password'], input[type='password']");

        private ILocator SubmitButton =>
            _page.GetByRole(AriaRole.Button, new() { Name = "Sign in" })
                .Or(_page.GetByRole(AriaRole.Button, new() { Name = "Login" }));

        public LoginPage(IPage page, ExecutionSettings settings)
        {
            _page = page;
            _settings = settings;
        }

        public async Task GotoAsync()
        {
            await _page.GotoAsync($"{_settings.BaseUrl}/_/#/login");
        }

        public async Task LoginAsync(string email, string password)
        {
            await GotoAsync();

            await Expect(IdentityInput).ToBeVisibleAsync();
            await IdentityInput.FillAsync(email);

            await Expect(PasswordInput).ToBeVisibleAsync();
            await PasswordInput.FillAsync(password);

            await Expect(SubmitButton).ToBeVisibleAsync();
            await SubmitButton.ClickAsync();
        }
    }
}
