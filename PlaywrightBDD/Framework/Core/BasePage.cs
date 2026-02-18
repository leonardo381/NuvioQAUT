using System;
using Microsoft.Playwright;

namespace Framework.Core
{
    public abstract class BasePage
    {
        protected IPage Page { get; }
        protected ElementExecutor Exec { get; }

        protected BasePage(IPage page, ElementExecutor executor)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            Exec = executor ?? throw new ArgumentNullException(nameof(executor));
        }
    }
}