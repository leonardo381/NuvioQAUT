using Framework.Core;
using Application.API;
using NUnit.Framework;

namespace Tests.API
{
    public class HealthTests : BaseTest
    {
        [Test]
        [Category("API")]
        [Category("Smoke")]
        public async Task Health_returns_200()
        {
            var pb = new PocketBaseApi(Settings.BaseUrl);
            var res = await pb.HealthAsync();
            Assert.That((int)res.StatusCode, Is.EqualTo(200), res.Body);
        }
    }
}