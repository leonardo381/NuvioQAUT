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
    }
}
