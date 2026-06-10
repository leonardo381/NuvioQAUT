# Testing Strategy

## Categories

### Smoke

Smoke tests are the default validation path.

They should be fast, stable, and avoid fragile app state. They should not create, update, or delete data.

Use:

```powershell
dotnet test --filter "Category=Smoke" --no-build
```

### API

API tests are non-browser checks.

They should use API-specific helpers or base classes when useful, but should not inherit UI/browser lifecycle just to access settings.

Use:

```powershell
dotnet test --filter "Category=API" --no-build
```

### UI

UI tests are browser tests.

They should move toward `Microsoft.Playwright.NUnit.PageTest` lifecycle and direct Playwright `Locator`/`Expect` usage.

### CRUD

CRUD tests create, read, update, or delete application data.

CRUD is not bad. It just needs an intentional local/disposable environment because it may alter test data.

Use only when that is intended:

```powershell
dotnet test --filter "Category=CRUD" --no-build
```

## Safety Direction

Avoid introducing extra categories like `Mutating` or `StateChanging` unless the user explicitly asks.

If protection is needed later, prefer simple NUnit-native or workflow-native mechanisms:

- `Category("CRUD")`
- NUnit `Explicit`
- a separate manual CI workflow

Prefer those over new custom guard classes.

## Refactor Reality

Preserving the current CRUD tests is not the highest priority if they block framework simplification.

They can be rewritten, simplified, or retired later if that helps remove unnecessary lifecycle/wrapper code. Any temporary breakage during a focused phase must be reported clearly.
