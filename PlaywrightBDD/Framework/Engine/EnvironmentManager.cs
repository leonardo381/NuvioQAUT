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

            // ADMIN credentials
            var adminUser = Environment.GetEnvironmentVariable("ADMIN_USER");
            if (!string.IsNullOrWhiteSpace(adminUser))
                settings.AdminUser = adminUser.Trim();

            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            if (!string.IsNullOrWhiteSpace(adminPassword))
                settings.AdminPassword = adminPassword.Trim();

            return settings;
        }
    }
}
