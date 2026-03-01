using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Engine
{
    public abstract class TestLifecycleManager
    {
        // ----------------------------
        // Global settings (once)
        // ----------------------------
        private static readonly ExecutionSettings GlobalSettings;

        static TestLifecycleManager()
        {
            GlobalSettings = EnvironmentManager.Load();
            Directory.CreateDirectory(GlobalSettings.ArtifactDir);
        }

        protected ExecutionSettings Settings => GlobalSettings;

        // ----------------------------
        // Shared engine + browser (per process)
        // ----------------------------
        private static readonly object _engineLock = new();
        private static Task? _engineInitTask;
        private static PlaywrightEngine? _sharedEngine;
        private static BrowserManager? _sharedBrowserManager;

        // ----------------------------
        // Per-test state
        // ----------------------------
        protected ContextManager? Ctx;

        private bool _needsUi;

        [SetUp]
        public async Task Setup()
        {
            _needsUi = ShouldUseUiForThisTest();
            if (!_needsUi)
                return;

            // Ensure engine + browser exist (shared across tests)
            await EnsureEngineAndBrowserAsync();

            if (_sharedBrowserManager?.Browser is null)
                throw new InvalidOperationException("Shared browser is null after initialization.");

            // Per-test context + page
            Ctx = new ContextManager(_sharedBrowserManager.Browser, Settings);
            await Ctx.CreateAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (!_needsUi)
                return;

            // Per-test cleanup only: context + page
            if (Ctx is not null)
            {
                await Ctx.DisposeAsync(TestContext.CurrentContext.Test.Name);
            }
        }

        // ----------------------------
        // Shared engine/browser init
        // ----------------------------
        private static Task EnsureEngineAndBrowserAsync()
        {
            // Fast path: already initializing or initialized
            var existing = _engineInitTask;
            if (existing != null)
                return existing;

            // Slow path: first thread wins and kicks off init
            lock (_engineLock)
            {
                if (_engineInitTask == null)
                {
                    _engineInitTask = InitializeEngineAndBrowserAsync();
                }

                return _engineInitTask;
            }
        }

        private static async Task InitializeEngineAndBrowserAsync()
        {
            _sharedEngine = new PlaywrightEngine();
            await _sharedEngine.InitializeAsync();

            _sharedBrowserManager = new BrowserManager(_sharedEngine, GlobalSettings);
            await _sharedBrowserManager.LaunchAsync();

            if (_sharedBrowserManager.Browser is null)
                throw new InvalidOperationException("BrowserManager.Browser is null after LaunchAsync().");
        }

        // ----------------------------
        // Category-driven UI/API toggle
        // ----------------------------
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

            // default to UI if unspecified
            return true;
        }
    }
}