using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Framework.Engine
{
    public class BrowserManager
    {
        private readonly PlaywrightEngine _engine;
        private readonly ExecutionSettings _settings;

        public IBrowser Browser { get; private set; } = default!;

        public BrowserManager(PlaywrightEngine engine, ExecutionSettings settings)
        {
            _engine = engine;
            _settings = settings;
        }

        public async Task LaunchAsync()
        {
            Browser = _settings.Browser.ToLower() switch
            {
                "firefox" => await _engine.Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings.Headless }),
                "webkit" => await _engine.Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings.Headless }),
                _ => await _engine.Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings.Headless })
            };
        }

        public async Task CloseAsync()
        {
            if (Browser != null)
                await Browser.CloseAsync();
        }
    }
}