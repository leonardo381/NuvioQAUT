using Application.API;
using Framework.Engine;

namespace Framework.Core
{
    public abstract class ApiTestBase
    {
        private static readonly ExecutionSettings GlobalSettings = EnvironmentManager.Load();

        protected ExecutionSettings Settings => GlobalSettings;

        protected PocketBaseApi CreatePocketBaseApi()
            => new(Settings.BaseUrl);
    }
}
