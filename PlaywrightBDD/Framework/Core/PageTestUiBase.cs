using Application.UI.PageTest;
using Framework.Engine;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace Framework.Core
{
    public abstract class PageTestUiBase : PageTest
    {
        private static readonly ExecutionSettings GlobalSettings = EnvironmentManager.Load();

        [SetUp]
        public void SetUpPageTestApp()
        {
            App = new PageTestNuvio(Page);
        }

        protected PageTestNuvio App { get; private set; } = null!;

        protected ExecutionSettings Settings => GlobalSettings;

        protected string ArtifactDir => Path.GetFullPath(Settings.ArtifactDir);

        protected Task<IResponse?> GotoBaseUrlAsync()
            => Page.GotoAsync(Settings.BaseUrl);

        protected Task<IResponse?> GotoLoginAsync()
            => Page.GotoAsync($"{Settings.BaseUrl.TrimEnd('/')}/_/#/login");
    }
}
