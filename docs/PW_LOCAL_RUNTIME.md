# Local Runtime

## Purpose

This project tests a running Nuvio application. The automation repository does not start Nuvio by itself during local test runs.

For a command-focused daily reference, see `docs/PW_COMMANDS.md`.

CI checks out the Nuvio application repository separately, starts it with Docker Compose, waits for it to answer on `http://127.0.0.1:8090/_/`, creates a PocketBase superuser, and then runs UI tests. Local execution must provide an equivalent reachable Nuvio instance before any browser or API runtime validation can pass.

## Required URL

Default:

```powershell
$env:BASE_URL = "http://127.0.0.1:8090"
```

If `BASE_URL` is not set, the framework defaults to:

```text
http://127.0.0.1:8090
```

## Reachability Check

Run this before browser/API runtime tests:

```powershell
$baseUrl = if ($env:BASE_URL) { $env:BASE_URL.TrimEnd("/") } else { "http://127.0.0.1:8090" }
Invoke-WebRequest -Uri "$baseUrl/_/" -UseBasicParsing -TimeoutSec 5
```

If that request cannot connect, runtime validation is blocked by app availability. Do not treat test failures from an unreachable app as framework failures.

## Safe Local Commands

These are safe framework checks:

```powershell
dotnet --version
dotnet restore
dotnet build
dotnet test --list-tests --no-build
```

When Nuvio is reachable, this smoke filter is non-mutating:

```powershell
dotnet test --filter "Category=Smoke" --no-build
```

Current Smoke coverage includes:

- API health check.
- PageTest login page form visibility check.

The login smoke navigates to the login page and asserts form elements. It does not submit credentials.

## Local Smoke Validation Status

Manual validation in a normal local PowerShell session passed on 2026-06-10:

```powershell
cd C:\Users\Leo\Documents\PW
Invoke-WebRequest http://127.0.0.1:8090/_/
dotnet restore
dotnet build
dotnet test --list-tests --no-build
dotnet test --filter "Category=Smoke" --no-build
```

Result:

- `http://127.0.0.1:8090/_/` returned HTTP 200.
- Restore passed.
- Build passed.
- Test discovery passed and listed 6 tests.
- `Category=Smoke` ran 2 tests.
- `Health_returns_200` passed.
- `LoginPage_ShouldExposeLoginForm_WithPageTestLifecycle` passed.
- Summary: total 2, failed 0, succeeded 2, skipped 0.

This validates the current `PageTestUiBase` + `PageTestLoginPage` + `PageTestLoginSmokeTests` path in a normal local PowerShell session.

Agent/sandbox note: the same PageTest smoke may fail inside the agent execution environment with `Microsoft.Playwright.PlaywrightException: spawn EPERM` while launching Chromium. That failure occurs before navigation or selector assertions and appears to be an agent/sandbox browser launch restriction, not a PageTest lifecycle, selector, or Nuvio runtime failure.

## Browser Install

After a successful build, install Playwright browsers if they are missing:

```powershell
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install
```

## Mutating Tests

Do not run these unless you explicitly intend to mutate the target Nuvio/PocketBase instance:

```powershell
dotnet test --filter "Category=UI"
dotnet test --filter "Category=CRUD"
```

The current UI CRUD tests create, update, and delete data. Use them only against an isolated or disposable Nuvio environment.

## Local And CI Differences

CI currently:

- Checks out `leonardo381/Nuvio` separately.
- Starts Nuvio with `docker compose up -d --build`.
- Waits for `http://127.0.0.1:8090/_/`.
- Creates a PocketBase superuser from repository secrets.
- Runs `dotnet test --filter "Category=UI" --no-build`.
- Uploads automation artifacts and Nuvio logs.

Local runs must start Nuvio separately before runtime tests. For safe PageTest lifecycle validation, prefer the Smoke filter first.
