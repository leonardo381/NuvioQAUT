using System;

namespace Framework.Engine
{
    public static class EnvironmentManager
    {
        public static ExecutionSettings Load()
        {
            var settings = new ExecutionSettings();

            // BASE_URL
            var baseUrl = Environment.GetEnvironmentVariable("BASE_URL");
            if (!string.IsNullOrWhiteSpace(baseUrl))
                settings.BaseUrl = baseUrl.Trim();

            // BROWSER
            var browser = Environment.GetEnvironmentVariable("BROWSER");
            if (!string.IsNullOrWhiteSpace(browser))
                settings.Browser = browser.Trim();

            // HEADLESS
            var headlessEnv = Environment.GetEnvironmentVariable("HEADLESS");
            if (!string.IsNullOrWhiteSpace(headlessEnv))
            {
                var v = headlessEnv.Trim().ToLowerInvariant();
                settings.Headless = v == "true" || v == "1" || v == "yes";
            }

            // TRACING
            var tracingEnv = Environment.GetEnvironmentVariable("TRACING");
            if (!string.IsNullOrWhiteSpace(tracingEnv))
            {
                var v = tracingEnv.Trim().ToLowerInvariant();
                settings.EnableTracing = v == "true" || v == "1" || v == "yes";
            }

            // SCREENSHOT_ON_FAILURE
            var screenshotEnv = Environment.GetEnvironmentVariable("SCREENSHOT_ON_FAILURE");
            if (!string.IsNullOrWhiteSpace(screenshotEnv))
            {
                var v = screenshotEnv.Trim().ToLowerInvariant();
                settings.ScreenshotOnFailure = v == "true" || v == "1" || v == "yes";
            }

            // TIMEOUT (ms)
            var timeoutEnv = Environment.GetEnvironmentVariable("TIMEOUT_MS");
            if (int.TryParse(timeoutEnv, out var timeoutMs))
                settings.TimeoutMs = timeoutMs;

            // SLOWMO (ms) - for debugging
            var slowMoEnv = Environment.GetEnvironmentVariable("SLOWMO_MS");
            if (int.TryParse(slowMoEnv, out var slowMoMs))
                settings.SlowMoMs = slowMoMs;

            // ARTIFACT_DIR
            var artifactDir = Environment.GetEnvironmentVariable("ARTIFACT_DIR");
            if (!string.IsNullOrWhiteSpace(artifactDir))
                settings.ArtifactDir = artifactDir.Trim();

            // ADMIN credentials
            var adminUser = Environment.GetEnvironmentVariable("ADMIN_USER");
            if (!string.IsNullOrWhiteSpace(adminUser))
                settings.AdminUser = adminUser.Trim();

            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            if (!string.IsNullOrWhiteSpace(adminPassword))
                settings.AdminPassword = adminPassword.Trim();

            // CRUD_COLLECTION (optional)
            var crudCollection = Environment.GetEnvironmentVariable("CRUD_COLLECTION");
            if (!string.IsNullOrWhiteSpace(crudCollection))
                settings.CrudCollection = crudCollection.Trim();

            return settings;
        }
    }
}