using Framework.Engine;
using Microsoft.Playwright;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core
{
    public abstract class BaseTest : TestLifecycleManager
    {
        protected IPage Page => Ctx.Page;

        //Create a single executor instance per test, with explicit dependencies
        private ElementExecutor? _executor;
        protected ElementExecutor Executor => _executor ??= new ElementExecutor(
            waiter: new Waiter(),
            retry: new RetryHandler()
        );

        [TearDown]
        public async Task CaptureScreenshotOnFailure()
        {
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

            var dir = Path.Combine(Settings.ArtifactDir, "screenshots");
            Directory.CreateDirectory(dir);

            var path = Path.Combine(dir, $"{TestContext.CurrentContext.Test.Name}.png");
            await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = path,
                FullPage = true
            });
        }
    }
}