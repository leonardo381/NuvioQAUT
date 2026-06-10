# Agent Instructions

## Project Purpose

This repository contains a C#/.NET 9 + NUnit + Microsoft Playwright automation framework for testing the Nuvio application.

It is a Nuvio-focused UI/API test project. It should not grow into a generic automation framework detached from the product it tests.

## Primary Refactor Direction

- Prefer `Microsoft.Playwright.NUnit` native lifecycle where possible.
- Prefer Playwright `Locator` APIs and `Expect` assertions over custom wait/retry/assert wrappers.
- Prefer small page objects/components over generic manager/executor layers.
- Keep Nuvio-specific app/page abstractions when they make tests clearer.
- Remove custom infrastructure after auditing usage and migration impact.

## Refactor Philosophy

- The goal is not to keep every old basic test green at all costs.
- The goal is to arrive at a simpler, more idiomatic Playwright/NUnit framework.
- Temporarily broken basic tests are acceptable during explicit refactor phases if the breakage is understood, reported, and has a recovery plan.
- Do not keep bad architecture only because old tests currently depend on it.

## Agent Workflow

1. Inspect first.
2. Understand current dependencies and call paths.
3. Propose or follow one focused phase.
4. Change only what the phase asks.
5. Avoid mixing unrelated work such as docs, CI, lifecycle, selectors, and test migration.
6. Run safe validation commands allowed by the phase.
7. Report changed files, commands, tests run/skipped, and risks.

## Hard Rules

- Do not introduce new guards, wrappers, managers, engines, or runners without explicit approval.
- Do not add safety abstractions unless the user explicitly asks for them.
- Do not migrate CRUD tests and clean up wrappers in the same phase.
- Do not run data-changing commands unless explicitly requested.
- Do not alter the Nuvio application from this repository.
- Do not expose credentials in docs, scripts, tests, logs, or final reports.
- Do not preserve a poor abstraction only to avoid touching old code.

## Safe Commands

These are generally safe read-only checks:

```powershell
dotnet --version
dotnet test --list-tests --no-build
```

Run build/restore only when the phase allows build output:

```powershell
dotnet restore
dotnet build
```

Do not run data-changing UI/CRUD tests unless the user explicitly asks and the target Nuvio/PocketBase instance is safe to mutate.

## Expected Report Format

At the end of each phase, report:

- Files changed.
- Commands run and result summary.
- Tests run or skipped.
- What broke, if anything.
- Why it broke.
- Risks or uncertainties.
- Next recommended phase.
