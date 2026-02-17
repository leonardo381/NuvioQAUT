namespace Framework.Engine
{
    public class ExecutionSettings
    {
        public string BaseUrl { get; set; } = "https://localhost";
        public bool Headless { get; set; } = true;
        public string Browser { get; set; } = "chromium";
        public int TimeoutMs { get; set; } = 30000;

        public bool EnableTracing { get; set; } = true;
        public bool ScreenshotOnFailure { get; set; } = true;

        // NEW: where CI will pick up artifacts from
        public string ArtifactDir { get; set; } = "artifacts";

        // for API + hybrid tests
        public string AdminUser { get; set; } = "";
        public string AdminPassword { get; set; } = "";

        // makes RecordsCrudTests easier
        public string CrudCollection { get; set; } = "";
    }
}