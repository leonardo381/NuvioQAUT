# Commands

PowerShell command reference for the PW framework and the local Nuvio app.

Do not run data-changing commands unless you intentionally want to mutate a local/disposable Nuvio/PocketBase instance.

## 1. Start Nuvio Locally

Nuvio repo:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
docker compose up -d
docker compose ps
docker compose logs -f --tail=100
```

Confirmed source: `C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio\docker-compose.yml`

The top-level compose file builds the current repo and exposes PocketBase/Nuvio on port `8090`.

Alternative local/staging compose template:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
Copy-Item deploy\env.backend.local.example deploy\env.backend.local
Copy-Item deploy\env.public.local.example deploy\env.public.local
docker compose -f deploy/docker-compose.base.example.yml build
docker compose -f deploy/docker-compose.base.example.yml up -d
```

Confirmed source: `deploy/README.md` and `deploy/docker-compose.base.example.yml`

## 2. Stop Or Restart Nuvio

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
docker compose restart
docker compose down
```

For the deploy template:

```powershell
docker compose -f deploy/docker-compose.base.example.yml down
```

Warning: `docker compose down -v` deletes volumes/data. Do not use it unless destroying local data is intentional.

## 3. Check Nuvio

```powershell
Invoke-WebRequest http://127.0.0.1:8090/_/
Invoke-WebRequest http://127.0.0.1:8090/api/health
```

## 4. Nuvio Build And Run Without Docker

The Nuvio repo has a PowerShell helper that imports `.env` and `ui/.env`, builds the UI, then serves the base example app:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
.\dev-serve.ps1
```

Confirmed source: `dev-serve.ps1`

Equivalent steps from the script:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio\ui
npm run build

cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
go run ./examples/base serve
```

Confirmed sources: `dev-serve.ps1`, `ui/package.json`, `README.md`

## 5. Nuvio Data, Snapshot, Populate, Restore

These commands were found during the documentation scan. They were not executed.

### Required Env Vars For Dev Data Tools

```powershell
$env:NUVIO_ALLOW_DEV_RESET = "1"
$env:PB_SUPERUSER_EMAIL = "<admin email>"
$env:PB_SUPERUSER_PASSWORD = "<admin password>"
$env:PB_URL = "http://127.0.0.1:8090"
$env:NUVIO_QA_BASE_URL = "http://127.0.0.1:8090"
```

Confirmed source: `.env.example`, `tools/dev/seed_operational_qa_data.go`, `tools/dev/create_qa_snapshot.go`, `tools/dev/restore_qa_snapshot.go`, `tools/dev/cmsqasnapshot/cms_qa_snapshot.go`

Do not document real values.

### Read-Only / Dry-Run Commands

Seed operational QA data dry-run:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
go run ./tools/dev/seed_operational_qa_data.go --websiteSlug "<website slug>"
```

Confirmed source: `tools/dev/seed_operational_qa_data.go`

Create full QA snapshot dry-run:

```powershell
go run ./tools/dev/create_qa_snapshot.go --name "<snapshot name>"
```

Confirmed source: `tools/dev/create_qa_snapshot.go`

Restore full QA snapshot dry-run:

```powershell
go run ./tools/dev/restore_qa_snapshot.go --name "operational_qa_baseline_v1"
```

Confirmed source: `tools/dev/restore_qa_snapshot.go`

Create CMS QA snapshot dry-run:

```powershell
go run ./tools/dev/create_cms_qa_snapshot.go --name "<snapshot name>" --websiteSlug "<website slug>" --assetsMode website
```

Confirmed source: `tools/dev/create_cms_qa_snapshot.go` and `tools/dev/cmsqasnapshot/cms_qa_snapshot.go`

Restore CMS QA snapshot dry-run:

```powershell
go run ./tools/dev/restore_cms_qa_snapshot.go --name "cms_v1_2026_06_04"
```

Confirmed source: `tools/dev/restore_cms_qa_snapshot.go` and `tools/dev/cmsqasnapshot/cms_qa_snapshot.go`

### Data-Changing Commands

These create or update local QA records. Run only against a disposable/local environment.

Seed operational QA data write mode:

```powershell
go run ./tools/dev/seed_operational_qa_data.go --websiteSlug "<website slug>" --confirm SEED_OPERATIONAL_QA_DATA
```

Confirmed source: `tools/dev/seed_operational_qa_data.go`

Scope found in source: Leads + Booking + Newsletter only. CMS content is not seeded by this command.

Create full QA snapshot write mode. Stop Nuvio first:

```powershell
go run ./tools/dev/create_qa_snapshot.go --name "<snapshot name>" --backendStopped --confirm CREATE_QA_SNAPSHOT
```

Confirmed source: `tools/dev/create_qa_snapshot.go`

Create CMS QA snapshot write mode. Stop Nuvio first:

```powershell
go run ./tools/dev/create_cms_qa_snapshot.go --name "<snapshot name>" --websiteSlug "<website slug>" --assetsMode website --backendStopped --confirm CREATE_CMS_QA_SNAPSHOT
```

Confirmed source: `tools/dev/create_cms_qa_snapshot.go` and `tools/dev/cmsqasnapshot/cms_qa_snapshot.go`

### Destructive / Reset / Restore Commands

These replace data and should be treated as destructive. Stop Nuvio first.

Restore full QA snapshot:

```powershell
go run ./tools/dev/restore_qa_snapshot.go --name "operational_qa_baseline_v1" --backendStopped --confirm RESTORE_QA_SNAPSHOT
```

Confirmed source: `tools/dev/restore_qa_snapshot.go`

Restore CMS QA snapshot:

```powershell
go run ./tools/dev/restore_cms_qa_snapshot.go --name "cms_v1_2026_06_04" --backendStopped --confirm RESTORE_CMS_QA_SNAPSHOT
```

Confirmed source: `tools/dev/restore_cms_qa_snapshot.go` and `tools/dev/cmsqasnapshot/cms_qa_snapshot.go`

Confirmed snapshot folders found:

- `dev_qa_snapshots\operational_qa_baseline_v1`
- `dev_qa_snapshots\cms\cms_v1_2026_06_04`

Not found during documentation scan:

- A command literally named "restrict restore".
- A separate generic "populate database" command beyond `seed_operational_qa_data.go`.
- A separate "demo data" command.
- A standalone PocketBase backup CLI command beyond the QA snapshot tools.

## 6. Framework Commands

PW repo:

```powershell
cd C:\Users\Leo\Documents\PW
dotnet --version
dotnet restore
dotnet build
dotnet test --list-tests --no-build
```

## 7. Safe Smoke / API

```powershell
dotnet test --filter "Category=Smoke" --no-build
dotnet test --filter "Category=API" --no-build
```

Smoke/API tests require Nuvio to be reachable when they call the app.
Authenticated Smoke is non-mutating, but it requires placeholder credentials to be configured locally:

```powershell
$env:ADMIN_USER = "<admin email>"
$env:ADMIN_PASSWORD = "<admin password>"
```

Do not document or commit real values.

## 8. CRUD Intentional Only

```powershell
dotnet test --filter "Category=CRUD" --no-build
```

CRUD may create, update, or delete test data. Run it only against a local/disposable environment.

## 9. Playwright Browser Install

Run from the PW repo after build:

```powershell
cd C:\Users\Leo\Documents\PW
dotnet build
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install chromium
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install --list
```

The script path is project-output-relative, not repo-root `.\bin\...`.

## 10. Framework Environment Variables

```powershell
$env:BASE_URL = "http://127.0.0.1:8090"
$env:ADMIN_USER = "<admin email>"
$env:ADMIN_PASSWORD = "<admin password>"
```

Do not include real values in docs, commits, logs, or reports.

## 11. Git Hygiene

```powershell
git status --short --untracked-files=all
git diff --stat
git diff
git add <files>
git commit -m "<message>"
```

## 12. Troubleshooting

Nuvio unreachable:

```powershell
Invoke-WebRequest http://127.0.0.1:8090/_/
docker compose ps
docker compose logs --tail=100
```

Wrong Playwright script path:

```powershell
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install --list
```

Playwright browsers missing:

```powershell
dotnet build
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install chromium
```

Agent/sandbox browser launch issues:

- `spawn EPERM` can happen in restricted agent environments.
- Recheck in a normal local PowerShell session before blaming selectors or framework code.

Connection refused:

- Confirm Nuvio is running.
- Confirm `BASE_URL`.
- Confirm the app is listening on port `8090`.

Database/snapshot command not found:

- Check `tools/dev`.
- Check `dev_qa_snapshots`.
- Do not invent a restore/seed command.

Restore/reset command risk:

- Stop Nuvio first.
- Use dry-run first.
- Confirm the target is local/disposable.
- Do not run restore commands against important data.
