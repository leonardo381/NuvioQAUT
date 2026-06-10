# Testing Strategy

## Smoke

Smoke tests are safe, fast default validation. They should not create, update, or delete data.

Run after Nuvio is reachable:

```powershell
dotnet test --filter "Category=Smoke" --no-build
```

## API

API tests are non-browser checks against Nuvio/PocketBase endpoints. They should use API-specific helpers or a future API-specific base, not a UI/browser base.

Run after Nuvio is reachable:

```powershell
dotnet test --filter "Category=API" --no-build
```

## UI

UI tests are browser tests. They should move toward Playwright/NUnit `PageTest` lifecycle.

Do not run all UI tests casually:

```powershell
dotnet test --filter "Category=UI" --no-build
```

Use this only when the target Nuvio environment is intended for that coverage.

## CRUD

CRUD tests create, read, update, and delete data. They are not bad; they just need an intentional environment.

Intended command:

```powershell
dotnet test --filter "Category=CRUD" --no-build
```

Current source should be checked before relying on this filter, because the visible users collection CRUD fixture is currently categorized as `UI` and `Regression`.

Do not run CRUD tests against an important or shared environment.

## Category Guidance

Useful categories:

- `Smoke`
- `API`
- `UI`
- `CRUD`
- `Regression`

Avoid extra categories such as `Mutating` or `StateChanging` unless the user explicitly asks.

If protection is needed later, prefer simple NUnit-native mechanisms:

- clear `[Category("CRUD")]`
- `[Explicit]`
- separate CI/manual workflow

Do not create custom guard classes unless a later phase explicitly requests them.
