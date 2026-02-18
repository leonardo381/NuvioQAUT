using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Engine
{
    public abstract class TestLifecycleManager
    {
        protected PlaywrightEngine Engine = default!;
        protected BrowserManager BrowserManager = default!;
        protected ContextManager Ctx = default!;

        protected ExecutionSettings Settings = default!;

        private bool _needsUi;
        private bool _uiInitialized;

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            Settings = EnvironmentManager.Load();
            Directory.CreateDirectory(Settings.ArtifactDir);
            await Task.CompletedTask;
        }

        [SetUp]
        public async Task Setup()
        {
            _needsUi = ShouldUseUiForThisTest();
            if (!_needsUi)
                return;

            await EnsureUiInitializedAsync();

            Ctx = new ContextManager(BrowserManager.Browser, Settings);
            await Ctx.CreateAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            if (!_needsUi)
                return;

            await Ctx.DisposeAsync(TestContext.CurrentContext.Test.Name);
        }

        [OneTimeTearDown]
        public async Task GlobalTearDown()
        {
            if (!_uiInitialized)
                return;

            await BrowserManager.CloseAsync();
            await Engine.DisposeAsync();
        }

        private async Task EnsureUiInitializedAsync()
        {
            if (_uiInitialized)
                return;

            Engine = new PlaywrightEngine();
            await Engine.InitializeAsync();

            BrowserManager = new BrowserManager(Engine, Settings);
            await BrowserManager.LaunchAsync();

            _uiInitialized = true;
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

            return true;
        }
    }
}