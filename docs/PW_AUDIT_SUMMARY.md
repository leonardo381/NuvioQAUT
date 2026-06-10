# Playwright Framework Audit Summary

## Executive Summary

The framework is workable and has useful Nuvio-specific structure, but it contains custom lifecycle and helper layers that duplicate native Playwright .NET and NUnit behavior. There is no separate custom runner. The effective harness is `BaseTest` + `TestLifecycleManager`.

The current harness correctly creates one isolated `BrowserContext` and `Page` per UI test. That principle should be preserved. However, browser/context/page lifecycle should likely move toward `Microsoft.Playwright.NUnit.PageTest` or `ContextTest` so Playwright/NUnit own generic lifecycle concerns.

## Strong Design Choices

- Per-test browser context and page isolation.
- Shared browser instance for performance.
- Category-based API tests currently avoid browser setup.
- `Nuvio` app entry point provides a clean test-facing API.
- Page objects and components keep test bodies readable.
- API client support is useful for health checks and future setup/cleanup.
- CI captures screenshots, Playwright traces, and Nuvio logs.

## High-Risk Design Choices

- Custom lifecycle duplicates `Microsoft.Playwright.NUnit` behavior.
- Shared browser and Playwright objects are not explicitly disposed.
- API tests inherit a UI-capable base class and rely on category detection.
- Broad action retries can hide real instability.
- Manual waits wrap Playwright locator actions that already auto-wait.
- UI CRUD tests mutate a reachable Nuvio/PocketBase instance.
- CI checks out the Nuvio repository without pinning a known app revision.

## Bloat Candidates

- `PlaywrightEngine`
- `BrowserManager`
- `ContextManager`
- `TestLifecycleManager`, if migrating to `PageTest`
- `Waiter`
- `RetryHandler`
- `RetryPolicy`
- `ElementExecutor`
- `UiAssert`
- `GenericAssert`
- Thin or unused pages/flows/components where no tests rely on them

## Dead/Stale Code Candidates

- Empty `Tests/Config/S1RunSettings.cs`
- Stale `.github/workflows/main.yml.desabled`
- Unused enums such as `BrowserType`, `TestEnvironment`, and `Timeout`
- `CrudCollection` setting and `CRUD_COLLECTION` environment variable
- Custom exception types that are not meaningfully thrown
- `DATA/*.xml` project include without tracked source data
- README sections that drift from the current code and CI flow

## Native Playwright/NUnit Duplication

The current framework manually creates Playwright, launches a browser, creates contexts and pages, starts/stops traces, and manages setup/teardown. Playwright .NET provides NUnit base classes that already cover browser reuse and per-test page/context creation.

Custom lifecycle may still be useful for project-specific settings and artifacts, but generic Playwright lifecycle should preferably belong to Playwright/NUnit.

## Fixture/Setup Review

Useful:

- `FixtureLifeCycle(LifeCycle.InstancePerTestCase)` protects per-test fields.
- Login setup keeps test methods focused.
- Per-test context/page creation is the right isolation model.

Questionable:

- Assembly-level and fixture-level parallel settings are mixed.
- API tests inherit from `BaseTest` even though they do not need page/browser access.
- Category detection is doing lifecycle routing that separate base classes would make clearer.

## Runner/Harness Verdict

There is no separate custom runner. The harness is `BaseTest` + `TestLifecycleManager`.

The harness is not fully justified for generic Playwright lifecycle because it duplicates `PageTest` / `ContextTest`. Keep a small Nuvio-specific base layer, but spike a migration to Playwright's NUnit base classes before adding more tests.

## Artifact/Diagnostics Review

- Screenshots on failure are valuable.
- Traces are valuable but currently generated for every UI test when tracing is enabled.
- CI uploads artifacts and Nuvio logs.
- Artifact file naming may collide for parameterized tests.
- Verbose artifact console logging could be simplified.
- Test result files such as `.trx` are not part of the current artifact story.

## Selector/Wait/Assertion Review

- Prefer role, label, and text locators where stable.
- Use `data-testid` or `data-pw` only when user-facing locators are not stable enough.
- Current UI selectors depend heavily on Nuvio/PocketBase internals such as CSS classes.
- Custom waits and broad retries duplicate Playwright locator auto-waiting.
- Custom UI assertions should be replaced gradually with Playwright `Expect`.

## Prioritized Recommendations

### P0: Must Fix Before Adding Tests

- Decide whether to migrate lifecycle to `Microsoft.Playwright.NUnit.PageTest`.
- Add explicit browser/Playwright disposal if keeping the current harness.
- Protect mutating UI/CRUD tests from accidental execution.

### P1: Should Simplify Soon

- Split UI and API base classes.
- Reduce or remove broad action retry wrappers.
- Replace custom UI assertions with Playwright `Expect`.
- Centralize settings and remove unused environment variables.

### P2: Nice Cleanup

- Clean up artifact naming and test result uploads.
- Remove stale workflow/docs after replacement is clear.
- Prune unused enums, exceptions, helpers, pages, flows, and model classes.
- Improve README accuracy.

### P3: Leave For Later

- Larger page object reshaping.
- Broader selector strategy changes requiring app-side test IDs.
- Reporting/dashboard improvements.
