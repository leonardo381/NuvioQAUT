using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Engine
{
    public abstract class TestLifecycleManager
    {
        //Global settings loaded once per process
        private static readonly ExecutionSettings GlobalSettings;

        static TestLifecycleManager()
        {
            GlobalSettings = EnvironmentManager.Load();
            Directory.CreateDirectory(GlobalSettings.ArtifactDir);
        }

        protected ExecutionSettings Settings => GlobalSettings;

        // Per-test
        protected PlaywrightEngine Engine = default!;
        protected BrowserManager BrowserManager = default!;
        protected ContextManager? Ctx;

        private bool _needsUi;

        [SetUp]
        public async Task Setup()
        {
            _needsUi = ShouldUseUiForThisTest();
            if (!_needsUi)
                return;

            // Per-test engine + browser + context
            Engine = new PlaywrightEngine();
            await Engine.InitializeAsync();

            BrowserManager = new BrowserManager(Engine, Settings);
            await BrowserManager.LaunchAsync();

            if (BrowserManager.Browser is null)
                throw new InvalidOperationException("BrowserManager.Browser is null after LaunchAsync().");

            Ctx = new ContextManager(BrowserManager.Browser, Settings);
            await Ctx.CreateAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (!_needsUi)
                return;

            if (Ctx is not null)
            {
                await Ctx.DisposeAsync(TestContext.CurrentContext.Test.Name);
            }

            if (BrowserManager is not null)
            {
                await BrowserManager.CloseAsync();
            }

            if (Engine is not null)
            {
                await Engine.DisposeAsync();
            }
        }

        private static bool ShouldUseUiForThisTest()
        {
            var cats = TestContext.CurrentContext.Test.Properties["Category"]
                .Cast<string>()
                .Select(c => c.ToLowerInvariant())
                .ToList();

            var isUi = cats.Contains("ui");
            var isApi = cats.Contains("api");

            if (isUi) return true;
            if (isApi) return false;

            // default to UI
            return true;
        }
    }
}