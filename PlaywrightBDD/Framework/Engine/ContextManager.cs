using Microsoft.Playwright;
using System.IO;
using System.Threading.Tasks;

namespace Framework.Engine
{
    public class ContextManager
    {
        private readonly IBrowser _browser;
        private readonly ExecutionSettings _settings;

        public IBrowserContext Context { get; private set; } = default!;
        public IPage Page { get; private set; } = default!;

        public ContextManager(IBrowser browser, ExecutionSettings settings)
        {
            _browser = browser;
            _settings = settings;
        }

        public async Task CreateAsync()
        {
            Context = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                BaseURL = _settings.BaseUrl
            });

            Page = await Context.NewPageAsync();
            Page.SetDefaultTimeout(_settings.TimeoutMs);

            if (_settings.EnableTracing)
            {
                await Context.Tracing.StartAsync(new TracingStartOptions
                {
                    Screenshots = true,
                    Snapshots = true,
                    Sources = true
                });
            }
        }

        public async Task DisposeAsync(string testName)
        {
            if (_settings.EnableTracing)
            {
                var traceDir = Path.Combine(_settings.ArtifactDir, "traces");
                Directory.CreateDirectory(traceDir);

                var tracePath = Path.Combine(traceDir, $"{testName}.zip");

                await Context.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = tracePath
                });
            }

            await Context.CloseAsync();
        }
    }
}