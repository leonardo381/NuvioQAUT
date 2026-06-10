# Agent Instructions

## Project Purpose

This repository is a C#/.NET 9 + NUnit + Playwright automation framework for Nuvio. It should stay focused on Nuvio UI/API validation, not become a generic automation framework.

## Refactor Direction

- Prefer `Microsoft.Playwright.NUnit` native lifecycle where possible.
- Prefer Playwright `Locator` APIs and `Expect` assertions over custom wait, retry, and assertion wrappers.
- Prefer small page objects/components over generic manager or executor layers.
- Keep Nuvio-specific app helpers when they make tests easier to read.
- Remove custom infrastructure only after verifying usages and migration impact.

## Working Pattern

- Inspect the current code first.
- Propose or follow one small phase at a time.
- Change only what the phase asks for.
- Do not mix docs, CI, lifecycle, selectors, and test migration in one phase.
- Do not migrate CRUD tests and clean up wrappers in the same phase.
- Run the safe validation commands allowed by the phase.
- Report changed files, commands, tests run/skipped, risks, and the next recommended phase.

## Hard Rules

- Do not introduce new guards, wrappers, managers, or framework layers without explicit approval.
- Do not run data-changing tests unless explicitly requested.
- Do not run `dotnet test --filter "Category=UI"` or `dotnet test --filter "Category=CRUD"` unless explicitly requested.
- Do not alter the Nuvio app from this repository.
- Do not expose real credentials in docs, code, examples, logs, or output.
- Do not delete or rename files unless the current phase explicitly allows it.

## Safe Commands

```powershell
dotnet --version
dotnet test --list-tests --no-build
```

When a phase allows build output:

```powershell
dotnet restore
dotnet build
```

Potentially data-changing commands:

```powershell
dotnet test --filter "Category=UI"
dotnet test --filter "Category=CRUD"
```

Use data-changing commands only against a Nuvio/PocketBase instance that is intended for that purpose.

## Report Format

At the end of each phase, report:

- Files changed.
- Commands run and results.
- Tests run or skipped.
- Risks or assumptions.
- Recommended next phase.
