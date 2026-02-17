using System;

namespace Framework.Engine
{
    public static class EnvironmentManager
    {
        public static ExecutionSettings Load()
        {
            var settings = new ExecutionSettings();

            settings.BaseUrl = Environment.GetEnvironmentVariable("BASE_URL") ?? settings.BaseUrl;
            settings.Browser = Environment.GetEnvironmentVariable("BROWSER") ?? settings.Browser;

            // HEADLESS
            settings.Headless = (Environment.GetEnvironmentVariable("HEADLESS") ?? "true").ToLower() != "false";

            // TRACING
            settings.EnableTracing = (Environment.GetEnvironmentVariable("TRACING") ?? "true").ToLower() != "false";

            // SCREENSHOT_ON_FAILURE
            settings.ScreenshotOnFailure =
                (Environment.GetEnvironmentVariable("SCREENSHOT_ON_FAILURE") ?? "true").ToLower() != "false";

            // TIMEOUT (ms)
            if (int.TryParse(Environment.GetEnvironmentVariable("TIMEOUT_MS"), out var timeoutMs))
                settings.TimeoutMs = timeoutMs;

            settings.ArtifactDir = Environment.GetEnvironmentVariable("ARTIFACT_DIR") ?? settings.ArtifactDir;

            settings.AdminUser = Environment.GetEnvironmentVariable("ADMIN_USER") ?? settings.AdminUser;
            settings.AdminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? settings.AdminPassword;
            //settings.CrudCollection = Environment.GetEnvironmentVariable("CRUD_COLLECTION") ?? settings.CrudCollection;

            return settings;
        }
    }
}