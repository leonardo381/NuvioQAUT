# Playwright Framework Contract

## Scope

This framework is not meant to become a generic automation framework. It is a Nuvio-focused Playwright automation framework.

Generic browser, context, page, and NUnit lifecycle behavior should be owned by Playwright/NUnit wherever practical. The custom project layer should stay focused on Nuvio-specific testing concerns.

## Project Layer Responsibilities

The custom project layer should own:

- App-specific navigation.
- Nuvio helpers and workflows.
- Page objects.
- Components.
- Test data strategy.
- Diagnostics and artifacts.
- CI integration.

## Behavioral Invariants

- Preserve per-test browser context and page isolation.
- Preserve readable tests.
- Preserve safe and deterministic test behavior.
- Preserve useful diagnostics for failures.
- Preserve CI artifact capture unless a phase explicitly changes it.

## Locator Strategy

- Prefer user-facing locators: role, label, text, and accessible names.
- Use `data-testid` or `data-pw` only where role, label, or text locators are not stable enough.
- Avoid coupling tests to implementation-only CSS classes when a stable user-facing locator exists.

## Waits, Retries, And Assertions

- Prefer Playwright locator auto-waiting.
- Prefer Playwright `Expect` for retrying UI assertions.
- Avoid broad retry wrappers around UI actions.
- Add explicit waits only when they express a real product state that Playwright cannot infer.
- Do not add new wrappers around Playwright without a clear project-specific reason.

## Destructive Test Safety

- Destructive or mutating tests must require explicit opt-in.
- Tests categorized as `Mutating` are skipped unless `ALLOW_MUTATING_TESTS=true` is set.
- CI should run Smoke tests by default and run mutating coverage only through an explicit intentional path.
- CRUD tests must not be run accidentally against a reachable shared Nuvio/PocketBase instance.
- Future cleanup/setup strategies should make test data ownership clear.

## UI And API Test Bases

API tests and UI tests should not share a confusing base class.

Current direction:

- `ApiTestBase` owns settings and simple API helper creation without browser/page setup.
- `PageTestUiBase` owns the new `PageTest` UI smoke path.
- Legacy UI/CRUD tests still use the old `BaseTest` + `TestLifecycleManager` path until they are migrated intentionally.

## What To Preserve During Refactors

- `Nuvio` as a test-facing app entry point unless a better Nuvio-specific facade replaces it.
- Page objects and components as the primary home for UI knowledge.
- API client support for setup, checks, and eventual cleanup.
- Screenshots, traces, and app logs as useful diagnostics.
- Existing test behavior unless the phase explicitly changes it.
