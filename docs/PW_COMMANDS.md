# PW Commands

## 1. Purpose

Daily command reference for the PW Playwright framework and the local Nuvio runtime it targets.

This is meant to be practical: start the same kind of Nuvio instance CI uses, check it is reachable, then run safe automation commands.

For runtime rationale and guardrails, see `docs/PW_LOCAL_RUNTIME.md`.

## 2. Assumed Paths

Nuvio application:

```powershell
C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
```

PW framework:

```powershell
C:\Users\Leo\Documents\PW
```

## 3. Open The CI-Like Nuvio Instance

This is the important Nuvio startup command. It mirrors the active CI workflow startup step.

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
docker compose up -d --build
```

The app should then be available at:

```text
http://127.0.0.1:8090/_/
```

Check readiness:

```powershell
Invoke-WebRequest http://127.0.0.1:8090/_/ -UseBasicParsing -TimeoutSec 5
```

Only use Docker details when debugging:

```powershell
docker compose ps
docker compose logs -f --tail=100
```

Stop the local CI-like instance:

```powershell
docker compose down
```

CI itself stops with `docker compose down -v`, but locally that can remove Docker volumes. Use `down -v` only when data loss is intentional.

## 4. Create The CI-Style Superuser

CI creates a PocketBase superuser after Nuvio is up.

PowerShell equivalent:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
$env:ADMIN_USER = "admin@example.com"
$env:ADMIN_PASSWORD = "secret"
docker compose exec -T nuvio /app/nuvio superuser upsert "$env:ADMIN_USER" "$env:ADMIN_PASSWORD"
```

Do not commit real credentials.

## 5. Run Nuvio With The Custom Build-And-Run Script

Use this when you want the local custom dev path instead of Docker.

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
.\dev-serve.ps1
```

What it does:

- Imports `.env` and `ui/.env`.
- Runs `npm run build` inside `ui`.
- Runs `go run ./examples/base serve`.
- Serves the app on the default Nuvio/PocketBase port, normally `http://127.0.0.1:8090`.

Manual equivalent:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio\ui
npm run build

cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
go run ./examples/base serve
```

## 6. Nuvio Runtime Environment

Common local values:

```powershell
$env:NUVIO_QA_BASE_URL = "http://127.0.0.1:8090"
$env:PB_URL = "http://127.0.0.1:8090"
$env:PB_SUPERUSER_EMAIL = "admin@example.com"
$env:PB_SUPERUSER_PASSWORD = "secret"
```

Dangerous/dev-only reset tools require explicit opt-in:

```powershell
$env:NUVIO_ALLOW_DEV_RESET = "1"
```

Unset it when finished:

```powershell
Remove-Item Env:\NUVIO_ALLOW_DEV_RESET -ErrorAction SilentlyContinue
```

## 7. Populate Operational QA Data

This is the custom DB populate command for operational QA data.

Scope: Leads, Booking, and Newsletter. CMS content is intentionally not included.

Dry-run first:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
$env:NUVIO_ALLOW_DEV_RESET = "1"
$env:NUVIO_QA_BASE_URL = "http://127.0.0.1:8090"
$env:PB_SUPERUSER_EMAIL = "admin@example.com"
$env:PB_SUPERUSER_PASSWORD = "secret"
go run ./tools/dev/seed_operational_qa_data.go --websiteSlug "<website-slug>"
```

Write mode:

```powershell
go run ./tools/dev/seed_operational_qa_data.go --websiteSlug "<website-slug>" --confirm SEED_OPERATIONAL_QA_DATA
```

You can target by id instead of slug:

```powershell
go run ./tools/dev/seed_operational_qa_data.go --websiteId "<website-id>" --confirm SEED_OPERATIONAL_QA_DATA
```

This command mutates the target Nuvio/PocketBase instance. Use it only against local/dev data.

## 8. Restore QA Snapshots

Available snapshots found locally:

- `operational_qa_baseline_v1`
- `cms/cms_v1_2026_06_04`

### Restore Full Operational pb_data

Dry-run:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
$env:NUVIO_ALLOW_DEV_RESET = "1"
go run ./tools/dev/restore_qa_snapshot.go --name operational_qa_baseline_v1
```

Write mode requires Nuvio/PocketBase to be stopped:

```powershell
go run ./tools/dev/restore_qa_snapshot.go --name operational_qa_baseline_v1 --backendStopped --confirm RESTORE_QA_SNAPSHOT
```

This replaces `pb_data` and creates a safety backup under `dev_qa_snapshots`.

### Restore CMS Snapshot

Dry-run:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
$env:NUVIO_ALLOW_DEV_RESET = "1"
go run ./tools/dev/restore_cms_qa_snapshot.go --name cms_v1_2026_06_04
```

Write mode requires Nuvio/PocketBase to be stopped:

```powershell
go run ./tools/dev/restore_cms_qa_snapshot.go --name cms_v1_2026_06_04 --backendStopped --confirm RESTORE_CMS_QA_SNAPSHOT
```

Optional guard if you know the expected website id:

```powershell
go run ./tools/dev/restore_cms_qa_snapshot.go --name cms_v1_2026_06_04 --websiteId "<website-id>" --backendStopped --confirm RESTORE_CMS_QA_SNAPSHOT
```

The CMS restore targets CMS-owned records and file storage. It is still mutating and should be local/dev only.

## 9. Create QA Snapshots

Create or update these only when deliberately refreshing local QA baselines.

Full `pb_data` snapshot dry-run:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
$env:NUVIO_ALLOW_DEV_RESET = "1"
go run ./tools/dev/create_qa_snapshot.go --name "<snapshot-name>"
```

Full `pb_data` snapshot write mode:

```powershell
go run ./tools/dev/create_qa_snapshot.go --name "<snapshot-name>" --backendStopped --confirm CREATE_QA_SNAPSHOT
```

CMS snapshot dry-run:

```powershell
go run ./tools/dev/create_cms_qa_snapshot.go --name "<snapshot-name>" --websiteSlug "<website-slug>"
```

CMS snapshot write mode:

```powershell
go run ./tools/dev/create_cms_qa_snapshot.go --name "<snapshot-name>" --websiteSlug "<website-slug>" --assetsMode all --backendStopped --confirm CREATE_CMS_QA_SNAPSHOT
```

`--assetsMode` accepts `all` or `website`; default is `all`.

## 10. PW Restricted Restore

Normal restore:

```powershell
cd C:\Users\Leo\Documents\PW
dotnet restore
```

Repo-local NuGet package cache:

```powershell
cd C:\Users\Leo\Documents\PW
$env:NUGET_PACKAGES = Join-Path (Get-Location).Path ".nuget\packages"
dotnet restore
```

Temporary NuGet package cache:

```powershell
cd C:\Users\Leo\Documents\PW
$env:NUGET_PACKAGES = Join-Path $env:TEMP "pw-nuget-packages"
dotnet restore
```

Clear the override:

```powershell
Remove-Item Env:\NUGET_PACKAGES -ErrorAction SilentlyContinue
```

Important: do not redirect `BaseIntermediateOutputPath` for this repo. A previous audit found that it can cause generated `obj` sources to be included unexpectedly and trigger duplicate assembly attribute errors, including duplicate `TargetFrameworkAttribute`.

## 11. PW Build And Discovery

```powershell
cd C:\Users\Leo\Documents\PW
dotnet --version
dotnet restore
dotnet build
dotnet test --list-tests --no-build
```

## 12. Safe Smoke Runtime Validation

Set the framework target URL:

```powershell
$env:BASE_URL = "http://127.0.0.1:8090"
```

Run only after the reachability check passes:

```powershell
cd C:\Users\Leo\Documents\PW
dotnet test --filter "Category=Smoke" --no-build
```

Current Smoke coverage includes:

- API health check.
- PageTest login page form visibility check.
- PageTest authenticated admin login check when `ADMIN_USER` and `ADMIN_PASSWORD` are configured.

The PageTest login form smoke navigates to the login page and asserts visible form elements. It does not submit credentials.

The authenticated login smoke submits admin credentials, asserts the browser leaves the login route, and asserts the login form is no longer visible. It does not create, update, or delete records. If `ADMIN_USER` or `ADMIN_PASSWORD` is missing, the authenticated smoke is skipped with a clear reason.

Manual local validation status:

- Validated in a normal local PowerShell session on 2026-06-10.
- `http://127.0.0.1:8090/_/` returned HTTP 200.
- `dotnet restore`, `dotnet build`, and `dotnet test --list-tests --no-build` passed.
- `dotnet test --filter "Category=Smoke" --no-build` ran 2 tests.
- `Health_returns_200` passed.
- `LoginPage_ShouldExposeLoginForm_WithPageTestLifecycle` passed.
- Summary: total 2, failed 0, succeeded 2, skipped 0.

The PageTest smoke path is runtime-validated locally for `PageTestUiBase` + `PageTestLoginPage` + `PageTestLoginSmokeTests`.

P1.3 adds a third Smoke test for authenticated admin login. That newer test still needs normal local PowerShell validation with valid `ADMIN_USER` and `ADMIN_PASSWORD`.

## 13. Do Not Run Accidentally

Potentially mutating automation:

```powershell
dotnet test --filter "Category=UI" --no-build
dotnet test --filter "Category=CRUD" --no-build
dotnet test --filter "Category=Mutating" --no-build
```

The UI/CRUD tests can create, update, or delete data in a reachable Nuvio/PocketBase instance. Current CRUD workflow tests are categorized as `CRUD` and `Mutating`.

The mutating guard skips these tests unless this opt-in is explicitly set:

```powershell
$env:ALLOW_MUTATING_TESTS = "true"
dotnet test --filter "Category=CRUD" --no-build
```

Unset it after the intentional run:

```powershell
Remove-Item Env:\ALLOW_MUTATING_TESTS -ErrorAction SilentlyContinue
```

Run mutating tests only against an isolated or disposable environment.

The active CI workflow runs `Category=Smoke` by default. `Category=CRUD` is available only through an intentional manual workflow dispatch path with `ALLOW_MUTATING_TESTS=true`.

## 14. Playwright Browser Installation

Run after `dotnet build` from the PW repository root if Playwright browsers are missing:

```powershell
cd C:\Users\Leo\Documents\PW
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install chromium
```

Use the first command for all required browsers. Use the second command when only Chromium is needed.

CI uses:

```powershell
pwsh .\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install --with-deps
```

## 15. PW Test Environment Variables

```powershell
$env:BASE_URL = "http://127.0.0.1:8090"
$env:HEADLESS = "true"
$env:TRACING = "true"
$env:SCREENSHOT_ON_FAILURE = "true"
$env:ARTIFACT_DIR = "artifacts"
$env:ADMIN_USER = "admin@example.com"
$env:ADMIN_PASSWORD = "secret"
```

Notes:

- `BASE_URL` defaults to `http://127.0.0.1:8090` if unset.
- `ADMIN_USER` and `ADMIN_PASSWORD` are needed for authenticated UI tests.
- Do not commit real credentials.

## 16. Recommended Local Flow

Safe PageTest smoke flow:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
docker compose up -d --build
Invoke-WebRequest http://127.0.0.1:8090/_/ -UseBasicParsing -TimeoutSec 5

cd C:\Users\Leo\Documents\PW
$env:BASE_URL = "http://127.0.0.1:8090"
$env:ADMIN_USER = "admin@example.com"
$env:ADMIN_PASSWORD = "secret"
dotnet restore
dotnet build
dotnet test --list-tests --no-build
dotnet test --filter "Category=Smoke" --no-build
```

Stop before `Category=Smoke` if the reachability check fails.

## 17. Troubleshooting

Nuvio unreachable at `127.0.0.1:8090`:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
docker compose ps
docker compose logs -f --tail=100
```

Need a clean CI-like container rebuild:

```powershell
docker compose up -d --build
```

Need the custom local build/run path:

```powershell
.\dev-serve.ps1
```

Playwright browsers missing:

```powershell
cd C:\Users\Leo\Documents\PW
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install
```

NuGet source connection refused at `127.0.0.1:9`:

- Check NuGet proxy/source configuration.
- Retry restore with network access available.
- Use the restricted restore commands above to isolate the package cache.

Duplicate `TargetFrameworkAttribute` or duplicate assembly attributes:

- Do not redirect `BaseIntermediateOutputPath`.
- Use normal `dotnet build`.
- If isolation is needed, override `NUGET_PACKAGES` only.

Smoke blocked by runtime availability:

- Confirm `BASE_URL`.
- Confirm `http://127.0.0.1:8090/_/` responds.
- Do not treat unreachable-app failures as framework failures.

PageTest smoke fails inside the agent with `spawn EPERM`:

- This has been observed in the agent/sandbox environment while launching Chromium.
- The failure occurs before navigation or selector assertions.
- The same `Category=Smoke` command passed in a normal local PowerShell session.
- Treat this as an agent/sandbox browser launch restriction unless it also reproduces in normal PowerShell.

## 18. Local Vs CI

Active CI currently:

- Checks out the PW automation repository.
- Checks out `leonardo381/Nuvio`.
- Starts Nuvio with `docker compose up -d --build`.
- Waits for `http://127.0.0.1:8090/_/`.
- Restores and builds `PlaywrightBDD`.
- Installs Playwright browsers with `install --with-deps`.
- Runs `dotnet test --filter "Category=Smoke" --no-build` by default on push and pull request.
- Allows intentional manual CRUD/Mutating coverage through `workflow_dispatch` with `run_mutating=true`.
- Sets `ALLOW_MUTATING_TESTS=true` only for the manual mutating path.
- Creates a PocketBase superuser with `docker compose exec -T nuvio /app/nuvio superuser upsert` only for the manual mutating path.
- Uploads automation artifacts and Nuvio logs.
- Stops containers with `docker compose down -v`.

Local runs should start Nuvio explicitly, prefer `Category=Smoke` first, and run mutating UI/CRUD filters only when that is intentional.
