using Application.UI.PageTest.Pages;
using Microsoft.Playwright;

namespace Application.UI.PageTest
{
    public sealed class PageTestNuvio
    {
        public PageTestNuvio(IPage page)
        {
            Login = new PageTestLoginPage(page);
        }

        public PageTestLoginPage Login { get; }
    }
}
