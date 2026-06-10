# Agent Instructions

## Project Purpose

This repository contains a C#/.NET 9 + NUnit + Microsoft Playwright automation framework for testing the Nuvio application. It is a Nuvio-focused UI and API test project, not a generic automation framework.

## Tech Stack

- .NET 9
- C#
- NUnit
- Microsoft.Playwright
- GitHub Actions
- Docker Compose for CI app startup
- PocketBase/Nuvio API support

## Current Architecture Summary

- `Tests/` contains NUnit UI and API tests.
- `Framework/Engine/` owns the current custom Playwright lifecycle.
- `Framework/Core/` owns base test/page helpers, waits, retries, and element execution wrappers.
- `Framework/Assertions/` owns custom assertion helpers.
- `Application/UI/` owns the Nuvio app entry point, page objects, components, flows, and UI contexts.
- `Application/API/` owns lightweight API client support for Nuvio/PocketBase.

The current harness is `BaseTest` + `TestLifecycleManager`. It creates one isolated `BrowserContext` and `Page` per UI test, but much of this duplicates `Microsoft.Playwright.NUnit.PageTest` / `ContextTest` behavior.

## Mandatory Workflow

- Audit before large changes.
- Work in small, explicit phases.
- Do not mix broad cleanup with feature or refactor work.
- Preserve existing behavior unless the current phase explicitly changes it.
- Keep changes scoped to the phase.
- Report changed files and command results after each phase.
- Call out skipped commands and why they were skipped.

## Safety Rules

- Do not run mutating UI or CRUD tests unless the user explicitly allows it.
- Do not run `dotnet test --filter "Category=UI"` or CRUD filters by accident.
- Do not delete old harness files during spike phases.
- Do not replace project-specific Nuvio abstractions with raw Playwright calls everywhere.
- Do not introduce more wrappers around Playwright without clear justification.
- Do not change CI behavior unless the phase is explicitly about CI.
- Do not change the Nuvio application from this repository.

## Preferred Direction

- Prefer `Microsoft.Playwright.NUnit.PageTest` for browser/context/page lifecycle.
- Keep Nuvio-specific helpers, page objects, components, diagnostics, and CI artifacts.
- Prefer Playwright `Locator` APIs and `Expect` assertions over custom waits, retries, and assertions.
- Split UI and API test bases long-term so API tests do not inherit a UI-capable base class.
- Keep per-test isolation as a core invariant.

## Build And Test Commands

Safe read-only checks:

```powershell
dotnet --version
dotnet test --list-tests --no-build
```

Potentially mutating or environment-dependent commands:

```powershell
dotnet test --filter "Category=UI"
dotnet test --filter "Category=CRUD"
```

Run mutating UI/CRUD tests only when explicitly requested and when the target Nuvio/PocketBase instance is safe to mutate.

Normal build commands, when a phase allows build output:

```powershell
dotnet restore
dotnet build
```

## Expected Agent Report Format

At the end of each phase, report:

- Files created or changed.
- Confirmation of the scope of changes.
- Commands run and exact result summary.
- Commands skipped and why.
- Any files that already existed and were updated instead of created.
- Assumptions, uncertainties, or follow-up risks.
