using NUnit.Framework;
using Framework.Core;
using Tests.Helpers;

namespace Tests.API
{
    public class HealthTests : ApiTestBase
    {
        [Test]
        [Category(TestCategories.API)]
        [Category(TestCategories.Smoke)]
        public async Task Health_returns_200()
        {
            var pb = CreatePocketBaseApi();
            var res = await pb.HealthAsync();
            Assert.That((int)res.StatusCode, Is.EqualTo(200), res.Body);
        }
    }
}
