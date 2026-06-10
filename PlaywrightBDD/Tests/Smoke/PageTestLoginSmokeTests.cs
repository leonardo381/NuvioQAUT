using Application.UI.PageTest.Pages;
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
            var loginPage = new PageTestLoginPage(Page);

            await loginPage.GotoAsync(Settings.BaseUrl);
            await loginPage.AssertLoadedAsync();
        }
    }
}
