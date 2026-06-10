using Framework.Engine;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Framework.Core
{
    public abstract class BaseTest : PageTest
    {
        protected ExecutionSettings Settings { get; } = EnvironmentManager.Load();

        private ElementExecutor? _executor;

        protected ElementExecutor Executor => _executor ??= new ElementExecutor(
            waiter: new Waiter(),
            retry: new RetryHandler()
        );

        [SetUp]
        public void ResetPerTestServices()
        {
            _executor = null;
        }
    }
}
