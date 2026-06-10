using Application.UI.Components;
using Application.UI.Flows;
using Application.UI.Pages;
using Framework.Engine;
using Microsoft.Playwright;
using System;

namespace Application.UI
{
    /// <summary>
    /// Test-scoped entry point for the Nuvio UI layer.
    /// Centralizes access to pages, flows and shared layout.
    /// </summary>
    public sealed class Nuvio
    {
        public IPage Page { get; }
        public ExecutionSettings Settings { get; }
        public AppShell Shell { get; }

        public Nuvio(IPage page, ExecutionSettings settings)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

            Shell = new AppShell(Page);
        }

        public LoginFlow Login => new LoginFlow(Page, Settings);

        public CollectionPage Collections => new CollectionPage(Page, Shell);

        public UsersPage Users => new UsersPage(Page, Shell);

        public UsersFlow UsersFlow => new UsersFlow(Page, Shell);
    }
}
