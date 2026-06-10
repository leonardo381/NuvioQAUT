# Nuvio Context

## What Nuvio Is Here

Nuvio is the application under test for this Playwright automation framework.

The framework workspace is:

```text
C:\Users\Leo\Documents\PW
```

The local Nuvio workspace is:

```text
C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
```

## Local URLs

Nuvio uses PocketBase/admin UI locally at:

```text
http://127.0.0.1:8090/_/
```

The base URL expected by tests is:

```text
http://127.0.0.1:8090
```

The login/admin route is expected around:

```text
/_/#/login
```

Nuvio must be running before browser or API smoke tests are executed.

## Local Versus CI

Local runs normally require starting Nuvio separately from this PW repository.

CI may start Nuvio through Docker Compose or a workflow-owned disposable instance. Do not assume local and CI startup are identical; inspect the workflow before changing CI behavior.

## Credentials

Use environment variables for credentials. Do not hardcode credentials and do not document real values.

Framework authenticated tests may use:

```powershell
$env:ADMIN_USER = "<admin email>"
$env:ADMIN_PASSWORD = "<admin password>"
```

The Nuvio repo contains `.env.example` entries for local/CI provisioning and tooling:

```powershell
$env:PB_SUPERUSER_EMAIL = "<admin email>"
$env:PB_SUPERUSER_PASSWORD = "<admin password>"
$env:PB_URL = "http://127.0.0.1:8090"
$env:NUVIO_QA_BASE_URL = "http://127.0.0.1:8090"
$env:NUVIO_ALLOW_DEV_RESET = "1"
```

Only set `NUVIO_ALLOW_DEV_RESET` when intentionally running Nuvio dev data tooling against a disposable/local environment.

## What Belongs In The Framework

Nuvio-specific page objects, components, and app helpers are useful when they make tests easier to read and maintain.

They should not duplicate Playwright lifecycle, locator waiting behavior, assertion behavior, retries, or generic action execution.
