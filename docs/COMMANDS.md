# Commands

PowerShell command reference for the PW framework and local Nuvio runtime.

## 1. Start Nuvio Locally

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
docker compose up -d
docker compose ps
docker compose logs -f --tail=100
```

## 2. Stop Or Restart Nuvio

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
docker compose restart
docker compose down
```

Be careful with:

```powershell
docker compose down -v
```

That can delete Docker volumes/data. Use it only when data loss is intentional.

## 3. Check Nuvio

```powershell
Invoke-WebRequest http://127.0.0.1:8090/_/ -UseBasicParsing -TimeoutSec 5
```

If this fails, browser/API runtime tests are blocked by app availability.

## 4. Framework Commands

```powershell
cd C:\Users\Leo\Documents\PW
dotnet --version
dotnet restore
dotnet build
dotnet test --list-tests --no-build
```

## 5. Safe Smoke And API

Run only after Nuvio is reachable:

```powershell
dotnet test --filter "Category=Smoke" --no-build
dotnet test --filter "Category=API" --no-build
```

Smoke and API tests should not create, update, or delete data.

## 6. CRUD Intentional Only

```powershell
dotnet test --filter "Category=CRUD" --no-build
```

CRUD tests may create, update, or delete test data. Run them only against a local, disposable, or otherwise intended Nuvio/PocketBase instance.

Check current test categories before relying on the filter; the visible users CRUD fixture is currently marked `UI` and `Regression`.

## 7. Playwright Browser Install

From the PW repository root:

```powershell
dotnet build
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install chromium
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install --list
```

The script path is under `PlaywrightBDD\bin\Debug\net9.0`, not the repository root `bin` directory.

## 8. Environment Variables

```powershell
$env:BASE_URL = "http://127.0.0.1:8090"
$env:ADMIN_USER = "<admin email>"
$env:ADMIN_PASSWORD = "<admin password>"
```

Do not include real credential values in docs, commits, logs, or examples.

## 9. Git Hygiene

```powershell
git status --short --untracked-files=all
git diff --stat
git diff
git add <files>
git commit -m "<message>"
```

## 10. Troubleshooting

Nuvio unreachable:

```powershell
cd C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
docker compose ps
docker compose logs -f --tail=100
```

Wrong Playwright script path:

```powershell
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install --list
```

Playwright browsers missing:

```powershell
.\PlaywrightBDD\bin\Debug\net9.0\playwright.ps1 install chromium
```

Agent/sandbox browser launch fails with `spawn EPERM`:

- Retry in a normal local PowerShell session.
- Treat it as an environment/browser-launch issue unless it also reproduces outside the agent.

Tests fail with connection refused:

- Confirm `BASE_URL`.
- Confirm `http://127.0.0.1:8090/_/` responds.
- Start or restart Nuvio before retrying runtime tests.
