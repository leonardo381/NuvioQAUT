using Application.API;
using Application.Shared.Models;
using Application.Shared.Builders;
using Framework.Core;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.API
{
    public class RecordsCrudTests : BaseTest
    {
        [Test]
        [Category("API")]
        public async Task CreateUser()
        {
            var scenario = XmlDataLoader.Load<CrudScenario>("CrudScenario.xml");

            var pb = new PocketBaseApi(Settings.BaseUrl);
            await pb.AdminLoginAsync(Settings.AdminUser, Settings.AdminPassword);

            var record = RecordBuilder.Create()
                .WithField("email", scenario.Email)
                .WithField("password", scenario.Password)
                .WithField("passwordConfirm", scenario.Password)
                .Build();

            var created = await pb.CreateRecordAsync(scenario.Collection, record);

            Assert.That(created.Id, Is.Not.Null.And.Not.Empty);
        }
    }
}