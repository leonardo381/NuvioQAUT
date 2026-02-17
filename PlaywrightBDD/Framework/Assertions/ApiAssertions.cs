using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Framework.Assertions
{
    public static class ApiAssert
    {
        public static void StatusCode(HttpResponseMessage response, HttpStatusCode expected)
        {
            Assert.That(response.StatusCode, Is.EqualTo(expected),
                $"Expected status code {expected} but got {response.StatusCode}");
        }

        public static async Task BodyContains(HttpResponseMessage response, string expected)
        {
            var content = await response.Content.ReadAsStringAsync();

            Assert.That(content, Does.Contain(expected),
                $"Expected response body to contain '{expected}'.");
        }
    }
}