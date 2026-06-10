using Framework.Engine;
using Microsoft.Playwright.NUnit;

namespace Framework.Core
{
    public abstract class BaseTest : PageTest
    {
        protected ExecutionSettings Settings { get; } = EnvironmentManager.Load();
    }
}
