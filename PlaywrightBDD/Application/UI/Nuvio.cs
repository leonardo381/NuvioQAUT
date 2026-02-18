using Application.UI.Components;
using Application.UI.Pages;
using Application.UI.Flows;
using Framework.Core;
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
        public ElementExecutor Exec { get; }
        public AppShell Shell { get; }

        public Nuvio(IPage page, ElementExecutor executor)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
            Exec = executor ?? throw new ArgumentNullException(nameof(executor));

            // Shared layout (sidebar / toolbar / toasts)
            Shell = new AppShell(Page, Exec);
        }

        // -------- Pages --------

        public UsersPage Users => new UsersPage(Page, Exec, Shell);
        public CollectionPage Collections => new CollectionPage(Page, Exec);

        // -------- Flows --------

        public LoginFlow Login => new LoginFlow(Page, Exec);
        public UsersFlow UsersFlow => new UsersFlow(Page, Exec);
    }
}