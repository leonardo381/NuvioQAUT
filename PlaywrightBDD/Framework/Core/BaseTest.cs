using Framework.Engine;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Framework.Core
{
    public abstract class BaseTest : TestLifecycleManager
    {
        protected IPage Page => ContextManager.Page;

        // Pages still have their own Executor
        private ElementExecutor? _executor;
        protected ElementExecutor Executor => _executor ??= new ElementExecutor(Page);

        [TearDown]
        public async Task CaptureScreenshotOnFailure()
        {
            // Only if this test actually used UI
            var cats = TestContext.CurrentContext.Test.Properties["Category"]
                .Cast<string>()
                .Select(c => c.ToLowerInvariant())
                .ToList();

            if (!cats.Contains("ui"))
                return;

            if (!Settings.ScreenshotOnFailure)
                return;

            if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
                return;

            var testName = MakeSafeFileName(TestContext.CurrentContext.Test.Name);

            var screenshotDir = Path.Combine(Settings.ArtifactDir, "screenshots");
            Directory.CreateDirectory(screenshotDir);

            var screenshotPath = Path.Combine(screenshotDir, $"{testName}.png");

            await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = screenshotPath,
                FullPage = true
            });
        }

        private static string MakeSafeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            return name;
        }
    }
}