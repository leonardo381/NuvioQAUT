/*using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Framework.Engine
{
    public abstract class TestLifecycleManager
    {
        protected PlaywrightEngine Engine = default!;
        protected BrowserManager BrowserManager = default!;
        protected ContextManager ContextManager = default!;
        protected ExecutionSettings Settings = default!;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            Settings = EnvironmentManager.Load();

            Engine = new PlaywrightEngine();
            await Engine.InitializeAsync();

            BrowserManager = new BrowserManager(Engine, Settings);
            await BrowserManager.LaunchAsync();
        }

        [SetUp]
        public async Task Setup()
        {
            ContextManager = new ContextManager(BrowserManager.Browser, Settings);
            await ContextManager.CreateAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await ContextManager.DisposeAsync(TestContext.CurrentContext.Test.Name);
        }

        [OneTimeTearDown]
        public async Task GlobalTearDown()
        {
            await BrowserManager.CloseAsync();
            await Engine.DisposeAsync();
        }
    }
}*/


using Microsoft.Playwright;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Engine
{
    public abstract class TestLifecycleManager
    {
        protected PlaywrightEngine Engine = default!;
        protected BrowserManager BrowserManager = default!;
        protected ContextManager ContextManager = default!;
        protected ExecutionSettings Settings = default!;

        private bool _needsUi;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            Settings = EnvironmentManager.Load();
        }

        [SetUp]
        public async Task Setup()
        {
            _needsUi = ShouldUseUiForThisTest();

            if (!_needsUi)
                return;

            Engine = new PlaywrightEngine();
            await Engine.InitializeAsync();

            BrowserManager = new BrowserManager(Engine, Settings);
            await BrowserManager.LaunchAsync();

            ContextManager = new ContextManager(BrowserManager.Browser, Settings);
            await ContextManager.CreateAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (!_needsUi)
                return;

            await ContextManager.DisposeAsync(TestContext.CurrentContext.Test.Name);

            await BrowserManager.CloseAsync();
            await Engine.DisposeAsync();
        }

        private static bool ShouldUseUiForThisTest()
        {
            var cats = TestContext.CurrentContext.Test.Properties["Category"]
                .Cast<string>()
                .Select(c => c.ToLowerInvariant())
                .ToList();

            var isUi = cats.Contains("ui");
            var isApi = cats.Contains("api");

            //needs browser
            if (isUi) return true;

            //no browser
            if (isApi) return false;

            //Default behavior
            return true;
        }
    }
}
