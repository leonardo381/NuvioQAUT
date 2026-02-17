using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Framework.Assertions
{
    public class UiAssert
    {
        private readonly IPage _page;

        public UiAssert(IPage page)
        {
            _page = page;
        }

        public async Task ElementVisible(string selector)
        {
            var visible = await _page.IsVisibleAsync(selector);
            Assert.That(visible, Is.True, 
                $"Expected element '{selector}' to be visible.");
        }

        public async Task ElementHidden(string selector)
        {
            var visible = await _page.IsVisibleAsync(selector);
            Assert.That(visible, Is.False, 
                $"Expected element '{selector}' to be hidden.");
        }

        public async Task TextContains(string selector, string expected)
        {
            var text = await _page.InnerTextAsync(selector);

            Assert.That(text, Does.Contain(expected),
                $"Expected '{selector}' to contain '{expected}', but was '{text}'.");
        }

        public void UrlContains(string expected)
        {
            Assert.That(_page.Url, Does.Contain(expected),
                $"Expected URL to contain '{expected}', but was '{_page.Url}'.");
        }
    }
}