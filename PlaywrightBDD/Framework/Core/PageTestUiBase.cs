using Framework.Engine;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.IO;
using System.Threading.Tasks;

namespace Framework.Core
{
    public abstract class PageTestUiBase : PageTest
    {
        private static readonly ExecutionSettings GlobalSettings = EnvironmentManager.Load();

        protected ExecutionSettings Settings => GlobalSettings;

        protected string ArtifactDir => Path.GetFullPath(Settings.ArtifactDir);

        protected Task<IResponse?> GotoBaseUrlAsync()
            => Page.GotoAsync(Settings.BaseUrl);

        protected Task<IResponse?> GotoLoginAsync()
            => Page.GotoAsync($"{Settings.BaseUrl.TrimEnd('/')}/_/#/login");
    }
}
