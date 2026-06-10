using Framework.Core;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Tests.Smoke
{
    [TestFixture]
    [Category("Smoke")]
    public sealed class PageTestLoginSmokeTests : PageTestUiBase
    {
        [Test]
        public async Task LoginPage_ShouldExposeLoginForm_WithPageTestLifecycle()
        {
            await GotoLoginAsync();

            var identityInput = Page.Locator("input[name='identity'], input[name='email'], input[type='email']").First;
            var passwordInput = Page.Locator("input[name='password'], input[type='password']").First;
            var submitButton = Page.GetByRole(AriaRole.Button, new() { Name = "Sign in" })
                .Or(Page.GetByRole(AriaRole.Button, new() { Name = "Login" }))
                .First;

            await Expect(identityInput).ToBeVisibleAsync();
            await Expect(passwordInput).ToBeVisibleAsync();
            await Expect(submitButton).ToBeVisibleAsync();
        }
    }
}
