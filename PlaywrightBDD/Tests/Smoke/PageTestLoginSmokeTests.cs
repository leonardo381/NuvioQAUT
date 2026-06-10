using Framework.Core;
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
            await App.Login.GotoAsync(Settings.BaseUrl);
            await App.Login.AssertLoadedAsync();
        }

        [Test]
        public async Task LoginPage_ShouldReachAuthenticatedArea_WithPageTestLifecycle()
        {
            if (string.IsNullOrWhiteSpace(Settings.AdminUser) ||
                string.IsNullOrWhiteSpace(Settings.AdminPassword))
            {
                Assert.Ignore("ADMIN_USER and ADMIN_PASSWORD are required for authenticated login smoke.");
            }

            await App.Login.LoginAsync(Settings.BaseUrl, Settings.AdminUser, Settings.AdminPassword);
            await App.Login.AssertAuthenticatedAsync();
        }
    }
}
