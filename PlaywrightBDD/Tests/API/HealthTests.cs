using Application.API;
using Framework.Engine;
using NUnit.Framework;

namespace Tests.API
{
    public class HealthTests
    {
        [Test]
        [Category("API")]
        [Category("Smoke")]
        public async Task Health_returns_200()
        {
            var settings = EnvironmentManager.Load();
            var pb = new PocketBaseApi(settings.BaseUrl);
            var res = await pb.HealthAsync();
            Assert.That((int)res.StatusCode, Is.EqualTo(200), res.Body);
        }
    }
}
