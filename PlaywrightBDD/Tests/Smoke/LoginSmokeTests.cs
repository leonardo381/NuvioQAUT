using Application.UI.PageTest;
using Framework.Core;

namespace Tests.Smoke
{
    public sealed class LoginSmokeTests : UiTestBase
    {
        [Test]
        [Category("Smoke")]
        public async Task LoginPage_ShouldExposeLoginForm_WithPageTestLifecycle()
        {
            var login = new PageTestLoginPage(Page);

            await login.GotoAsync(Settings.BaseUrl);
            await login.AssertLoadedAsync();
        }
    }
}
