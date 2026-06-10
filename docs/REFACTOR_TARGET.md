# Refactor Target

## Main Objective

Replace custom browser/context/page lifecycle with `Microsoft.Playwright.NUnit` where possible.

The project should use Playwright closer to its strengths: native test lifecycle, auto-waiting locators, web-first assertions, tracing/artifacts where useful, and simple NUnit categories.

## Keep

- Nuvio-specific abstractions that keep tests clearer.
- Useful page objects/components.
- API client support if it remains small and valuable.
- Readable app-level helpers/facade if they reduce duplication.
- Diagnostics/artifacts that are actually useful.
- CI smoke validation if it gives real confidence.

## Remove Or Simplify After Audit

- Custom `BrowserManager`, `ContextManager`, and `PlaywrightEngine` lifecycle if `PageTest` replaces them.
- `Waiter` if Playwright locators already handle the waiting.
- `RetryHandler` if it retries too broadly or hides real instability.
- `ElementExecutor` if it wraps basic locator actions without adding product value.
- Custom UI assertions if Playwright `Expect` covers them better.
- Unused enums, exceptions, settings, files, and comments.
- Duplicate setup/base classes once tests no longer need them.

## Important Principle

Do not preserve code just because current basic tests depend on it.

If code is the wrong abstraction, it can be removed or replaced in a focused refactor phase. Any resulting test failures must be reported with:

- which tests broke
- why they broke
- whether the breakage is expected
- how the next phase should recover

## Preferred Refactor Phases

1. Audit current code after documentation exists.
2. Identify the smallest direct path to Playwright/NUnit native lifecycle.
3. Replace lifecycle/wrapper usage in the simplest UI path.
4. Keep or rebuild only Nuvio-specific page abstractions that improve readability.
5. Split API and UI only if it simplifies the structure.
6. Rewrite or retire basic CRUD tests later if they block simplification.
7. Remove old lifecycle/wrapper code once the new path is established, even if old basic tests need rewriting.

## Anti-Goals

- Do not build a second framework next to the old one.
- Do not add safety abstractions unless requested.
- Do not create new terminology that makes the repo harder to understand.
- Do not add docs/process as a substitute for simplification.
- Do not over-prioritize preserving weak/basic tests.
- Do not replace useful Nuvio domain language with raw Playwright calls everywhere.

## Target Outcome

Tests should read like Nuvio user/workflow checks, while the low-level mechanics stay close to Playwright/NUnit:

```csharp
await App.Login.GotoAsync(BaseUrl);
await App.Login.SignInAsync(adminUser, adminPassword);
await App.Dashboard.ExpectLoadedAsync();
```

The app/page abstractions should express Nuvio intent. They should not own browser lifecycle, generic waits, generic retries, or generic action execution.
