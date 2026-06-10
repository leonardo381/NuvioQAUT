# Architecture

## Current Main Areas

- `PlaywrightBDD/Framework`: custom Playwright lifecycle, base classes, wrappers, assertions, diagnostics, and settings.
- `PlaywrightBDD/Application/API`: Nuvio/PocketBase API helpers.
- `PlaywrightBDD/Application/UI`: Nuvio UI entry point, pages, components, flows, and contexts.
- `PlaywrightBDD/Tests/API`: API tests.
- `PlaywrightBDD/Tests/UI`: browser UI tests, currently including users collection CRUD coverage.
- `PlaywrightBDD/Tests/Smoke`: target area for smoke tests if present.
- `.github/workflows`: GitHub Actions workflow for running tests against Nuvio.

## Current Shape

The existing framework has custom lifecycle pieces:

- `Framework/Engine/PlaywrightEngine.cs`
- `Framework/Engine/BrowserManager.cs`
- `Framework/Engine/ContextManager.cs`
- `Framework/Engine/TestLifecycleManager.cs`

`BaseTest` currently sits on top of that custom lifecycle. Existing page objects/components may depend on custom wrappers such as `ElementExecutor`, `Waiter`, `RetryHandler`, and custom assertions.

Some of this structure is useful Nuvio-specific organization. Some of it may be legacy or removable once Playwright/NUnit native lifecycle and locator behavior are used more directly.

## Desired Target

- NUnit remains the test runner.
- `Microsoft.Playwright.NUnit` should own browser/context/page lifecycle where possible.
- API tests should not depend on UI/browser base classes.
- UI tests should use `PageTest`-based base classes where possible.
- Page objects should use `IPage`, `ILocator`, and Playwright `Expect` directly.
- A Nuvio app facade is allowed only if it keeps tests readable and does not become a new generic framework.

## Avoid

- Growing `Engine`, `Manager`, or `Executor` layers.
- Creating wrappers for basic Playwright actions unless justified.
- Keeping duplicate lifecycle systems long-term.
- Adding framework concepts just to avoid simplifying old code.

## Target Tree Example

This is an aspiration, not a guarantee about the current tree:

```text
PlaywrightBDD/
  Application/
    API/
    UI/
      Pages/
      Components/
      Nuvio.cs
  Framework/
    Core/
      ApiTestBase.cs
      UiTestBase.cs
      TestCategories.cs
  Tests/
    API/
    Smoke/
    UI/
```

Remove old lifecycle and wrapper code only after a focused audit and migration path.
