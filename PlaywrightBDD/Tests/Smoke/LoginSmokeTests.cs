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

        [Test]
        [Category("Smoke")]
        public async Task LoginPage_ShouldReachAuthenticatedArea_WithPageTestLifecycle()
        {
            if (string.IsNullOrWhiteSpace(Settings.AdminUser) ||
                string.IsNullOrWhiteSpace(Settings.AdminPassword))
            {
                Assert.Ignore("ADMIN_USER and ADMIN_PASSWORD are required for authenticated smoke.");
            }

            var login = new PageTestLoginPage(Page);

            await login.LoginAsync(Settings.BaseUrl, Settings.AdminUser, Settings.AdminPassword);
            await login.AssertAuthenticatedAsync();
        }
    }
}
