using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace PlaywrightBDD;

public class Tests
{
    private IPlaywright _playwright;
    private IBrowser _browser;
    private IPage _page;

    [SetUp]
    public async Task Setup()
    {
        _playwright = await Playwright.CreateAsync();
        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = false // see the browser
        });

        _page = await _browser.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _browser.CloseAsync();
        _playwright.Dispose();
    }

    [Test]
    public async Task User_can_open_Google_homepage()
    {
        // GIVEN the user opens Google
        await _page.GotoAsync("https://www.google.com");

        // THEN the title should contain Google
        Assert.That(await _page.TitleAsync(), Does.Contain("Google"));
    }
}
