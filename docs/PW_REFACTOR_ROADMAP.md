# Playwright Framework Refactor Roadmap

This roadmap defines planned phases only. Do not implement refactors during documentation phases.

## D0: Documentation Bootstrap

Goal:

- Add stable instructions and audit notes for future agents.

Files likely to change:

- `AGENTS.md`
- `docs/PW_AUDIT_SUMMARY.md`
- `docs/PW_FRAMEWORK_CONTRACT.md`
- `docs/PW_REFACTOR_ROADMAP.md`

What not to change:

- Production code.
- Test code.
- CI behavior.
- Nuvio application code.

Build/test commands:

```powershell
dotnet --version
dotnet test --list-tests --no-build
```

Manual checklist:

- Confirm only documentation files changed.
- Confirm no UI/CRUD tests were run.
- Confirm command results are reported.

## P0: Playwright NUnit Lifecycle Spike

Goal:

- Prove whether `Microsoft.Playwright.NUnit.PageTest` can replace the custom browser/context/page lifecycle while preserving per-test isolation and artifacts.

Status:

- P0 lifecycle spike added a clean `PageTest` path without deleting the old custom harness.
- `PageTestUiBase` + `PageTestLoginPage` + `PageTestLoginSmokeTests` are runtime-validated in a normal local PowerShell session as of 2026-06-10.
- Manual `Category=Smoke` result: total 2, failed 0, succeeded 2, skipped 0.
- The agent/sandbox environment may still fail to launch Chromium with `Microsoft.Playwright.PlaywrightException: spawn EPERM`; that occurs before navigation/selectors and does not currently invalidate the local PageTest smoke result.

Files likely to change:

- `PlaywrightBDD.csproj`
- `Framework/Core/BaseTest.cs`
- `Framework/Engine/TestLifecycleManager.cs`
- `Framework/Engine/PlaywrightEngine.cs`
- `Framework/Engine/BrowserManager.cs`
- `Framework/Engine/ContextManager.cs`
- A small spike test fixture

What not to change:

- Do not delete old harness files during the spike.
- Do not rewrite page objects.
- Do not change CRUD test behavior.

Build/test commands:

```powershell
dotnet restore
dotnet build
dotnet test --list-tests --no-build
```

Manual checklist:

- Verify one context/page per UI test.
- Verify browser reuse or acceptable performance.
- Verify screenshots/traces can still be captured.
- Verify API tests do not start a browser.

## P1: Split UiTestBase And ApiTestBase

Goal:

- Remove category-driven lifecycle routing by separating UI and API base classes.

Files likely to change:

- `Framework/Core/BaseTest.cs`
- New `UiTestBase`
- New `ApiTestBase`
- `Tests/UI/*`
- `Tests/API/*`

What not to change:

- Do not change test assertions or product workflows.
- Do not broaden test coverage in the same phase.

Build/test commands:

```powershell
dotnet build
dotnet test --list-tests --no-build
```

Manual checklist:

- API tests do not expose `Page`.
- UI tests still get isolated page/context behavior.
- Categories remain useful for filtering.

## P1: Replace Custom Waits/Retries/Assertions Gradually

Goal:

- Prefer Playwright locator auto-waiting and `Expect` over custom wrappers.

Files likely to change:

- `Framework/Core/ElementExecutor.cs`
- `Framework/Core/Waiter.cs`
- `Framework/Core/RetryHandler.cs`
- `Framework/Diagnostics/RetryPolicy.cs`
- `Framework/Assertions/*`
- Page objects and components that call wrappers

What not to change:

- Do not mass-rewrite all selectors at once.
- Do not remove wrappers until no active code needs them.

Build/test commands:

```powershell
dotnet build
dotnet test --list-tests --no-build
```

Manual checklist:

- Replace one vertical path at a time.
- Confirm failures stay readable.
- Confirm no broad retry remains around normal click/fill actions.

## P1/P2: Protect CRUD/Destructive Tests

Goal:

- Prevent accidental mutation of shared Nuvio/PocketBase instances.

Files likely to change:

- `Tests/UI/*`
- Test category constants
- Test base setup
- CI workflow or runsettings
- README/agent docs

What not to change:

- Do not remove CRUD coverage.
- Do not run destructive tests without explicit approval.

Build/test commands:

```powershell
dotnet build
dotnet test --list-tests --no-build
```

Manual checklist:

- Destructive tests require explicit opt-in.
- Local and CI behavior are documented.
- Test data cleanup strategy is clear.

## P2: Artifact And Diagnostics Cleanup

Goal:

- Make screenshots, traces, logs, and test results consistent and useful.

Files likely to change:

- Lifecycle/base test files
- `ContextManager` or replacement artifact hooks
- `.github/workflows/main.yml`
- `.gitignore`

What not to change:

- Do not reduce failure diagnosability.
- Do not change app startup behavior in the same phase unless required.

Build/test commands:

```powershell
dotnet build
dotnet test --list-tests --no-build
```

Manual checklist:

- Artifact names are unique and filesystem-safe.
- CI uploads test results and app logs.
- Trace policy is intentional: all tests or failures only.

## P2: CI Hardening

Goal:

- Make CI representative, deterministic, and easier to diagnose.

Files likely to change:

- `.github/workflows/main.yml`
- README
- Possible runsettings file

What not to change:

- Do not change framework lifecycle in the same phase.
- Do not change Nuvio app code from this repository.

Build/test commands:

```powershell
dotnet build
dotnet test --list-tests --no-build
```

Manual checklist:

- Nuvio checkout is pinned or intentionally tracks latest.
- CI setup matches documented local setup.
- Secrets and admin setup are documented.

## P2/P3: Dead Code Cleanup

Goal:

- Remove stale files only after replacement phases prove they are unused.

Files likely to change:

- Empty config files
- Unused enums
- Unused exception/helper classes
- Stale workflow files
- Stale README sections

What not to change:

- Do not delete old harness files before lifecycle migration is complete.
- Do not remove project-specific Nuvio abstractions just because they are thin.

Build/test commands:

```powershell
dotnet build
dotnet test --list-tests --no-build
```

Manual checklist:

- Confirm no references remain with `rg`.
- Confirm git diff is only cleanup.
- Confirm docs reflect removed files.

## P3: README Refresh

Goal:

- Make README accurate after lifecycle, CI, and safety decisions settle.

Files likely to change:

- `README`
- Possibly docs under `docs/`

What not to change:

- Do not use README refresh to sneak in code or CI behavior changes.

Build/test commands:

```powershell
dotnet test --list-tests --no-build
```

Manual checklist:

- README matches actual commands.
- README warns about mutating UI/CRUD tests.
- README links to agent docs and framework contract.
