using System;
using Microsoft.Playwright;
using Framework.Engine;

namespace Framework.Core
{
    public abstract class BasePage
    {
        protected IPage Page { get; }
        protected ElementExecutor Exec { get; }
        protected ExecutionSettings Settings { get; }

        protected BasePage(IPage page, ElementExecutor executor, ExecutionSettings settings)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            Exec = executor ?? throw new ArgumentNullException(nameof(executor));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }
    }
}