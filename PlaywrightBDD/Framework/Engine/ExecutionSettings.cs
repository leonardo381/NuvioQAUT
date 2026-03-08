namespace Framework.Engine
{
    public class ExecutionSettings
    {
        public string BaseUrl { get; set; } = "http://127.0.0.1:8090";
        public bool Headless { get; set; } = false;
        public string Browser { get; set; } = "chromium";
        public int TimeoutMs { get; set; } = 30000;
        public bool EnableTracing { get; set; } = true;
        public bool ScreenshotOnFailure { get; set; } = true;
        public string ArtifactDir { get; set; } = "artifacts";
        public string AdminUser { get; set; } = "";
        public string AdminPassword { get; set; } = "";
        public string CrudCollection { get; set; } = "";
        public int SlowMoMs { get; set; } = 0;
    }
}