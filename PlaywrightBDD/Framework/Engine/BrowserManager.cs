using Microsoft.Playwright;
using System;
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
            Console.WriteLine(
                $"[BrowserManager] Launching '{_settings.Browser}' " +
                $"(Headless={_settings.Headless}, SlowMo={_settings.SlowMoMs}ms)...");

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = _settings.Headless,
                SlowMo = _settings.SlowMoMs > 0 ? _settings.SlowMoMs : 0
            };

            Browser = _settings.Browser.ToLowerInvariant() switch
            {
                "firefox" => await _engine.Playwright.Firefox.LaunchAsync(launchOptions),
                "webkit"  => await _engine.Playwright.Webkit.LaunchAsync(launchOptions),
                _         => await _engine.Playwright.Chromium.LaunchAsync(launchOptions)
            };
        }

        public async Task CloseAsync()
        {
            if (Browser != null)
                await Browser.CloseAsync();
        }
    }
}