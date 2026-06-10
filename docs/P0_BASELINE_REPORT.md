# P0 Baseline Report

Date: 2026-06-10

## Scope

P0 established the real source/build/test-discovery baseline before any Playwright/NUnit lifecycle refactor.

No production code, test code, CI files, or Nuvio app files were changed.

## Current Branch

```text
main
```

Recent commits:

```text
7d25a04 upade documents
435a386 add documents
988a0aa Refactor for class noise reduction
8355683 ExecutionSettings integration
adcf14d Add README
```

## Working Tree Status

Initial `git status --short --untracked-files=all` output was empty.

There were no unexpected refactor leftovers visible in the working tree before this report was created.

## Source Test Files Found

Test folder files:

```text
PlaywrightBDD/Tests/ParallelConfig.cs
PlaywrightBDD/Tests/API/HealthTests.cs
PlaywrightBDD/Tests/Config/S1RunSettings.cs
PlaywrightBDD/Tests/Helpers/TestCategories.cs
PlaywrightBDD/Tests/Helpers/TestDataFactory.cs
PlaywrightBDD/Tests/UI/CollectionCrudTests.cs
```

Actual source test files:

```text
PlaywrightBDD/Tests/API/HealthTests.cs
PlaywrightBDD/Tests/UI/CollectionCrudTests.cs
```

## Source Test Classes And Methods

`PlaywrightBDD/Tests/API/HealthTests.cs`

- `HealthTests : BaseTest`
- `Health_returns_200`
- Categories: `API`, `Smoke`

`PlaywrightBDD/Tests/UI/CollectionCrudTests.cs`

- `UsersCollectionCrudTests : BaseTest`
- Fixture categories: `UI`, `Regression`
- `CreateUser_ShouldAppearInGrid`
- `ReadUser_ShouldMatchGridValues`
- `UpdateUser_ShouldReflectNewValuesInGrid`
- `DeleteUser_ShouldRemoveRowFromGrid`

Current source does not mark the CRUD fixture with `Category("CRUD")`.

## Pre-Clean No-Build Discovery

Command:

```powershell
dotnet test --list-tests --no-build
```

Result: passed, but this was pre-clean no-build discovery and was not trusted as source of truth.

It listed 7 tests:

```text
CreateUser_ShouldAppearInGrid
DeleteUser_ShouldRemoveRowFromGrid
ReadUser_ShouldMatchGridValues
UpdateUser_ShouldReflectNewValuesInGrid
Health_returns_200
LoginPage_ShouldExposeLoginForm_WithPageTestLifecycle
LoginPage_ShouldReachAuthenticatedArea_WithPageTestLifecycle
```

The two `LoginPage_*_WithPageTestLifecycle` tests were stale binary tests. They do not exist in the current visible source.

## Build Cleanup Performed

`dotnet clean` succeeded with 0 warnings and 0 errors.

It removed the old compiled project assembly and stale Playwright-related binaries, including:

```text
PlaywrightBDD/bin/Debug/net9.0/PlaywrightBDD.dll
PlaywrightBDD/bin/Debug/net9.0/Microsoft.Playwright.NUnit.dll
PlaywrightBDD/bin/Debug/net9.0/Microsoft.Playwright.TestAdapter.dll
```

After `dotnet clean`, `dotnet test --list-tests --no-build` failed as expected because `PlaywrightBDD.dll` no longer existed.

Manual `Remove-Item` of `PlaywrightBDD/bin` and `PlaywrightBDD/obj` was attempted after verifying both paths resolved inside:

```text
C:\Users\Leo\Documents\PW\PlaywrightBDD
```

The sandboxed removal hit `Access denied` on stale generated folders/files under `bin/Debug/net9.0`, including `.playwright`, localization folders, screenshots, and traces.

An escalated removal was then run only for:

```text
PlaywrightBDD/bin
PlaywrightBDD/obj
```

That succeeded. No source files were deleted.

## Locked Processes Found Or Stopped

After the first build failed with access-denied copy errors, related processes were listed:

```text
dotnet 14392 C:\Program Files\dotnet\dotnet.exe
dotnet 17716 C:\Program Files\dotnet\dotnet.exe
dotnet 24408 C:\Program Files\dotnet\dotnet.exe
```

No `testhost` or `vstest.console` processes were listed.

No processes were stopped because the visible `Get-Process` data was not enough to prove those `dotnet` processes were clearly related to this build/test session.

## Restore Result

Command:

```powershell
dotnet restore
```

Result: passed.

Warning:

```text
NU1900: Error occurred while getting package vulnerability data:
Unable to load the service index for source https://api.nuget.org/v3/index.json.
```

The restore completed despite the warning.

## Build Result

An initial build after partial cleanup failed with `MSB3021` access-denied copy errors into old `bin/Debug/net9.0` subfolders. Example:

```text
Unable to copy file ... to "bin\Debug\net9.0\ru\Microsoft.TestPlatform.CoreUtilities.resources.dll".
Access to the path ... is denied.
```

After removing `PlaywrightBDD/bin` and `PlaywrightBDD/obj`, the final build passed.

Command:

```powershell
dotnet build
```

Result: passed.

Warnings:

```text
NU1900: Error occurred while getting package vulnerability data:
Unable to load the service index for source https://api.nuget.org/v3/index.json.

CS1998: PlaywrightBDD/Framework/Engine/PlaywrightEngine.cs(15,27):
This async method lacks 'await' operators and will run synchronously.
```

Errors: none.

## Post-Build Test Discovery

Command:

```powershell
dotnet test --list-tests --no-build
```

Result: passed.

It listed 5 tests:

```text
CreateUser_ShouldAppearInGrid
DeleteUser_ShouldRemoveRowFromGrid
ReadUser_ShouldMatchGridValues
UpdateUser_ShouldReflectNewValuesInGrid
Health_returns_200
```

## Actual Current Test List

Actual current compiled source tests:

1. `Health_returns_200`
2. `CreateUser_ShouldAppearInGrid`
3. `ReadUser_ShouldMatchGridValues`
4. `UpdateUser_ShouldReflectNewValuesInGrid`
5. `DeleteUser_ShouldRemoveRowFromGrid`

The stale PageTest tests were removed from discovery after clean rebuild.

## Source/Binary Mismatch

Pre-clean binary state did not match source.

Mismatch found:

- Source had 5 test methods.
- Pre-clean no-build discovery listed 7 tests.
- The extra tests were:
  - `LoginPage_ShouldExposeLoginForm_WithPageTestLifecycle`
  - `LoginPage_ShouldReachAuthenticatedArea_WithPageTestLifecycle`
- The stale output also contained `Microsoft.Playwright.NUnit.dll`, but the current `.csproj` does not reference `Microsoft.Playwright.NUnit`.

After deleting build outputs and rebuilding, source and binary discovery aligned at 5 tests.

## Current Custom Lifecycle Components Found

Current custom lifecycle path:

- `Framework/Core/BaseTest.cs`
- `Framework/Engine/TestLifecycleManager.cs`
- `Framework/Engine/PlaywrightEngine.cs`
- `Framework/Engine/BrowserManager.cs`
- `Framework/Engine/ContextManager.cs`
- `Framework/Engine/EnvironmentManager.cs`
- `Framework/Engine/ExecutionSettings.cs`

Observed behavior:

- `BaseTest` inherits `TestLifecycleManager`.
- `TestLifecycleManager` loads global settings once.
- `TestLifecycleManager` creates a shared Playwright engine/browser for UI tests.
- `ContextManager` creates per-test browser context/page.
- API tests avoid browser setup through category detection.

## Current Wrapper Components Found

Current wrapper/helper components:

- `Framework/Core/ElementExecutor.cs`
- `Framework/Core/Waiter.cs`
- `Framework/Core/RetryHandler.cs`
- `Framework/Assertions/UIAssertions.cs` (`UiAssert`)
- `Framework/Assertions/BaseAssertions.cs` (`GenericAssert`)
- `Framework/Diagnostics/RetryPolicy.cs`

Current Nuvio UI model dependencies:

- `Application/UI/Nuvio.cs` accepts `IPage`, `ElementExecutor`, and `ExecutionSettings`.
- `BasePage`, pages, flows, and components use `ElementExecutor` directly or indirectly.

## Microsoft.Playwright.NUnit / PageTest References

Source search found no current references to:

```text
Microsoft.Playwright.NUnit
PageTest
```

`PlaywrightBDD.csproj` currently references:

```text
Microsoft.Playwright 1.58.0
NUnit 4.2.2
NUnit3TestAdapter 4.6.0
Microsoft.NET.Test.Sdk 17.12.0
```

It does not reference `Microsoft.Playwright.NUnit`.

## API Runtime Check

`dotnet test --filter "Category=API" --no-build` was not run.

Reason: this phase did not establish `BASE_URL` reachability, and no browser/UI/CRUD/runtime app tests were required to prove source/binary alignment.

## Recommended Next Phase

Baseline is now clean enough to proceed.

Recommended next phase: `P1 - PageTest Vertical Slice`.

Scope:

- Add `Microsoft.Playwright.NUnit`.
- Create a minimal UI base using `PageTest`.
- Create one clean login page object using `IPage`, `ILocator`, and Playwright `Expect`.
- Create one non-mutating login smoke test.
- Do not wire `ElementExecutor`, `Waiter`, `RetryHandler`, `UiAssert`, or `GenericAssert` into the new path.
- Do not migrate CRUD tests in the same phase.
