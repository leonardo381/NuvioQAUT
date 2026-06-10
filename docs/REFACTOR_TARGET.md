# Refactor Target

## Main Objective

Replace custom browser/context/page lifecycle with `Microsoft.Playwright.NUnit` where possible.

The goal is simplification: fewer custom lifecycle layers, fewer wrappers, and clearer Nuvio-focused tests.

## Keep

- Useful Nuvio page objects/components.
- API client support if useful.
- Readable app-level helpers.
- CI smoke validation.
- Per-test browser context/page isolation.

## Remove Or Simplify After Audit

- `BrowserManager`, `ContextManager`, and `PlaywrightEngine` if `PageTest` replaces their lifecycle responsibilities.
- `Waiter` if Playwright locators already handle waiting.
- `RetryHandler` if it retries too broadly.
- `ElementExecutor` if it wraps basic locator actions without adding value.
- Custom UI assertions if Playwright `Expect` covers them better.
- Unused enums, exceptions, settings, files, and stale workflow artifacts.

## Do Not Remove Yet

- Anything still needed by current compiling tests.
- Anything not audited.

## Preferred Refactor Phases

1. Audit current code after documentation exists.
2. Prove a `PageTest` path with one safe smoke test.
3. Migrate one small page object to `Locator`/`Expect`.
4. Split API tests away from UI base classes if needed.
5. Migrate simple UI tests first.
6. Migrate or retire CRUD tests later.
7. Remove old lifecycle/wrapper code only when unused.

## Anti-Goals

- Do not build a second framework next to the old one.
- Do not add safety abstractions unless requested.
- Do not create new terminology that makes the repo harder to understand.
- Do not add docs/process as a substitute for simplification.
- Do not migrate CRUD tests and remove wrappers in the same phase.
