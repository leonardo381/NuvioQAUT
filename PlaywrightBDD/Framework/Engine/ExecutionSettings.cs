namespace Framework.Engine
{
    public class ExecutionSettings
    {
        // Point directly to the admin UI, not the demo wrapper
        public string BaseUrl { get; set; } = "https://pocketbase.io/_/#/login";
        public bool Headless { get; set; } = false;
        public string Browser { get; set; } = "chromium";
        public int TimeoutMs { get; set; } = 30000;
        public bool EnableTracing { get; set; } = true;
        public bool ScreenshotOnFailure { get; set; } = true;
        // Where CI will pick up artifacts from
        public string ArtifactDir { get; set; } = "artifacts";
        // for API + hybrid tests
        public string AdminUser { get; set; } = "";
        public string AdminPassword { get; set; } = "";
        // makes RecordsCrudTests easier
        public string CrudCollection { get; set; } = "";
        // Optional: slow motion for debugging (ms between actions)
        public int SlowMoMs { get; set; } = 0;
    }
}