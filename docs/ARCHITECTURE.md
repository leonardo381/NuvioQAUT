# Architecture

## Current Main Areas

- `PlaywrightBDD/Framework`: current framework-level lifecycle, core helpers, assertions, and settings.
- `PlaywrightBDD/Application/API`: Nuvio/PocketBase API support.
- `PlaywrightBDD/Application/UI`: Nuvio UI app entry points, page objects, components, flows, and contexts.
- `PlaywrightBDD/Tests/API`: API/non-browser NUnit tests.
- `PlaywrightBDD/Tests/UI`: browser UI tests, including current CRUD coverage.
- `PlaywrightBDD/Tests/Smoke`: smoke tests if present in the current branch.
- `.github/workflows`: CI startup and test execution.

## Current Shape

The existing framework contains custom Playwright lifecycle pieces and helper layers. Some page objects/components depend on those wrappers.

Typical current responsibilities include:

- Custom browser/context/page lifecycle under `Framework/Engine`.
- Base test/page helpers under `Framework/Core`.
- Custom element execution, waits, retries, and assertion helpers.
- Nuvio-specific UI abstractions under `Application/UI`.

This should be treated neutrally during inspection. Some pieces may be valuable; others are legacy abstractions built with a Selenium-style mindset and may duplicate what Playwright already provides.

## Desired Target

- NUnit remains the test runner.
- `Microsoft.Playwright.NUnit` should own browser/context/page lifecycle where possible.
- API tests should not depend on UI/browser base classes.
- UI tests should use `PageTest`-based bases where possible.
- Page objects/components should use `IPage`, `ILocator`, and Playwright `Expect` directly.
- A Nuvio app facade/page model is allowed when it keeps tests readable.
- The framework should stay product-focused instead of becoming a generic automation framework.

## Warnings

Avoid:

- Growing `Engine`, `Manager`, `Executor`, or runner layers.
- Creating wrappers for basic Playwright actions unless strongly justified.
- Keeping duplicate lifecycle systems long-term.
- Adding more framework concepts just to avoid touching old code.
- Preserving weak abstractions only because current basic tests depend on them.

Old tests may be rewritten or temporarily broken during focused refactor phases if that produces a cleaner architecture. Breakage must always be reported with the reason and recovery plan.

## Target Tree Example

This is an aspiration, not a guarantee of the current tree:

```text
PlaywrightBDD/
  Application/
    API/
      NuvioApiClient.cs
    UI/
      NuvioApp.cs
      Pages/
      Components/
  Framework/
    Settings/
    Diagnostics/
  Tests/
    API/
    Smoke/
    UI/
```

In the target shape, framework code contains only shared infrastructure that is still useful. Product behavior lives in Nuvio app/page/component abstractions.

## Current Post-P3 Split

- `Health_returns_200` is now a plain NUnit API test and no longer inherits `BaseTest`.
- It loads settings directly through `EnvironmentManager.Load()` and constructs `PocketBaseApi` with `settings.BaseUrl`.
- The old `BaseTest` / `TestLifecycleManager` path remains only for legacy UI/CRUD tests for now.
- The clean `UiTestBase : PageTest` path remains the validated direction for new UI smoke coverage.
