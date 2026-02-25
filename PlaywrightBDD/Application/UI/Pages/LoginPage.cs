using Framework.Core;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Pages
{
    public sealed class LoginPage : BasePage
    {
        private ILocator IdentityInput =>
            Page.Locator("input[name='identity'], input[name='email'], input[type='email']");

        private ILocator PasswordInput =>
            Page.Locator("input[name='password'], input[type='password']");

        private ILocator SubmitButton =>
            Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" })
                .Or(Page.GetByRole(AriaRole.Button, new() { Name = "Login" }));

        public LoginPage(IPage page, ElementExecutor executor)
            : base(page, executor)
        {
        }

        public async Task GotoAsync()
        {
            await Page.GotoAsync("http://127.0.0.1:8090/_/#/login");
        }

        public async Task LoginAsync(string email, string password)
        {
            await GotoAsync();

            await Exec.FillAsync(IdentityInput, email);
            await Exec.FillAsync(PasswordInput, password);
            await Exec.ClickAsync(SubmitButton);
        }
    }
}
