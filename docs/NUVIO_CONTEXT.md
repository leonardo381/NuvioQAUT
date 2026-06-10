# Nuvio Context

## Application Under Test

Nuvio is the application under test. This repository tests Nuvio through UI and API automation and should not change Nuvio application code.

Nuvio uses PocketBase/admin UI locally at:

```text
http://127.0.0.1:8090/_/
```

The default base URL expected by tests is:

```text
http://127.0.0.1:8090
```

The admin login path is expected around:

```text
/_/#/login
```

## Local Workspaces

Nuvio application:

```text
C:\Users\Leo\Documents\Nuvio\NuvioCMS\Nuvio
```

PW automation framework:

```text
C:\Users\Leo\Documents\PW
```

## Runtime Requirement

Nuvio must be running before browser or API smoke tests are run. CI may start Nuvio through Docker Compose, but local runs must start it separately.

## Credentials

Use environment variables:

```powershell
$env:ADMIN_USER = "<admin email>"
$env:ADMIN_PASSWORD = "<admin password>"
```

Rules:

- Do not hardcode credentials.
- Do not document real credential values.
- Do not commit local credential files.

## Useful Abstractions

Nuvio-specific app helpers and page objects are useful when they keep tests readable. They should not duplicate Playwright browser/context/page lifecycle.
