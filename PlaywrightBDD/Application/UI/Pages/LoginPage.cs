using Framework.Core;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Application.UI.Pages
{
    public sealed class LoginPage : BasePage
    {
        private IFrameLocator Frame => Page.FrameLocator("iframe");

        private ILocator IdentityInput =>
            Frame.Locator("input[type='email']");

        private ILocator PasswordInput =>
            Frame.Locator("input[type='password']");

        private ILocator SubmitButton =>
            Frame.GetByRole(AriaRole.Button, new() { Name = "Login" });

        public LoginPage(IPage page, ElementExecutor executor)
            : base(page, executor)
        {
        }

        public async Task GotoAsync()
        {
            await Page.GotoAsync("https://pocketbase.io/demo/");
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
