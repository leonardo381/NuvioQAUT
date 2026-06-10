# P2b Wrapper Dependency Audit

## Scope

P2b audited the remaining Framework wrapper chain after `BaseTest` moved to `Microsoft.Playwright.NUnit.PageTest`.

No `Application` or `Tests` files were changed.

## Remaining Wrapper Chain

```text
BaseTest.Executor
  -> ElementExecutor
     -> Waiter
     -> RetryHandler
        -> RetryPolicy
        -> RetryExceededException
           -> FrameworkException
```

## Classification

### ElementExecutor

Status: active and externally referenced.

Framework references:

- `PlaywrightBDD/Framework/Core/BaseTest.cs`
- `PlaywrightBDD/Framework/Core/BasePage.cs`

Application/Test blockers:

- `PlaywrightBDD/Tests/UI/CollectionCrudTests.cs`
- `PlaywrightBDD/Application/UI/Nuvio.cs`
- `PlaywrightBDD/Application/UI/Pages/LoginPage.cs`
- `PlaywrightBDD/Application/UI/Pages/CollectionPage.cs`
- `PlaywrightBDD/Application/UI/Pages/UsersPage.cs`
- `PlaywrightBDD/Application/UI/Pages/DashboardPage.cs`
- `PlaywrightBDD/Application/UI/Flows/LoginFlow.cs`
- `PlaywrightBDD/Application/UI/Flows/UsersFlow.cs`
- `PlaywrightBDD/Application/UI/Components/AppShell.cs`
- `PlaywrightBDD/Application/UI/Components/Base/UIComponent.cs`
- `PlaywrightBDD/Application/UI/Components/Toolbar.cs`
- `PlaywrightBDD/Application/UI/Components/GridComponent.cs`
- `PlaywrightBDD/Application/UI/Components/ModalComponent.cs`
- `PlaywrightBDD/Application/UI/Components/SidebarMenu.cs`
- `PlaywrightBDD/Application/UI/Components/ToastsComponent.cs`

Deletion: blocked until `Application/UI` no longer accepts or stores `ElementExecutor`.

### Waiter

Status: only used by another wrapper and by `BaseTest` construction of `ElementExecutor`.

References:

- `PlaywrightBDD/Framework/Core/ElementExecutor.cs`
- `PlaywrightBDD/Framework/Core/BaseTest.cs`

Deletion: blocked until `ElementExecutor` is removed from `BaseTest` and `Application/UI`.

### RetryHandler

Status: only used by another wrapper and by `BaseTest` construction of `ElementExecutor`.

References:

- `PlaywrightBDD/Framework/Core/ElementExecutor.cs`
- `PlaywrightBDD/Framework/Core/BaseTest.cs`

Deletion: blocked until `ElementExecutor` is removed.

### RetryPolicy

Status: only used by `RetryHandler`.

References:

- `PlaywrightBDD/Framework/Core/RetryHandler.cs`
- `PlaywrightBDD/Framework/Diagnostics/RetryPolicy.cs`

Deletion: blocked until `RetryHandler` is removed.

### RetryExceededException

Status: only used by `RetryHandler`.

References:

- `PlaywrightBDD/Framework/Core/RetryHandler.cs`
- `PlaywrightBDD/Framework/Diagnostics/Exceptions/RetryExceededException.cs`

Deletion: blocked until `RetryHandler` is removed.

### FrameworkException

Status: only used by `RetryExceededException`.

References:

- `PlaywrightBDD/Framework/Diagnostics/Exceptions/RetryExceededException.cs`
- `PlaywrightBDD/Framework/Diagnostics/Exceptions/FrameworkException.cs`

Deletion: blocked until `RetryExceededException` is removed.

## ElementExecutor Method Review

### ClickAsync

Current behavior:

- Calls `Waiter.VisibleAsync(locator, timeoutMs)`.
- Calls `RetryHandler.ExecuteAsync("Click", () => locator.ClickAsync())`.

Playwright-native replacement:

```csharp
await Assertions.Expect(locator).ToBeVisibleAsync();
await locator.ClickAsync();
```

For many buttons/links, `await locator.ClickAsync()` may be enough because Playwright locators already auto-wait for actionability.

Assessment: duplicates Playwright locator auto-waiting and adds broad action-level retry that can hide instability.

### FillAsync

Current behavior:

- Calls `Waiter.VisibleAsync(locator, timeoutMs)`.
- Calls `RetryHandler.ExecuteAsync("Fill", () => locator.FillAsync(value))`.

Playwright-native replacement:

```csharp
await Assertions.Expect(locator).ToBeVisibleAsync();
await locator.FillAsync(value);
```

Assessment: duplicates Playwright locator actionability checks and retries a basic locator action.

### PressAsync

Current behavior:

- Calls `Waiter.VisibleAsync(locator, timeoutMs)`.
- Calls `RetryHandler.ExecuteAsync("Press", () => locator.PressAsync(key))`.

Current external usage: none found outside `ElementExecutor`.

Playwright-native replacement:

```csharp
await locator.PressAsync(key);
```

Assessment: currently dead surface, but keep until the wrapper is removed as a whole to avoid unrelated API churn.

## Recommended Migration Path

Migrate the old `Application/UI` model away from `ElementExecutor` in small slices.

Recommended first target:

```text
PlaywrightBDD/Application/UI/Pages/LoginPage.cs
```

Reason:

- It is small.
- A clean reference already exists in `PlaywrightBDD/Application/UI/PageTest/PageTestLoginPage.cs`.
- It exercises the same pattern needed elsewhere: constructor should accept `IPage`, locators should be page-owned, and actions/assertions should use `ILocator` plus `Assertions.Expect`.

Target shape:

```csharp
public sealed class LoginPage
{
    private readonly IPage _page;

    private ILocator IdentityInput => _page.Locator("input[name='identity'], input[name='email'], input[type='email']");
    private ILocator PasswordInput => _page.Locator("input[name='password'], input[type='password']");

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task FillCredentialsAsync(string usernameOrEmail, string password)
    {
        await Assertions.Expect(IdentityInput).ToBeVisibleAsync();
        await IdentityInput.FillAsync(usernameOrEmail);

        await Assertions.Expect(PasswordInput).ToBeVisibleAsync();
        await PasswordInput.FillAsync(password);
    }
}
```

After `LoginPage` proves the pattern, migrate:

1. `Application/UI/Flows/LoginFlow.cs`
2. `Application/UI/Nuvio.cs`
3. `Application/UI/Components/Toolbar.cs`
4. `Application/UI/Components/SidebarMenu.cs`
5. `Application/UI/Components/ModalComponent.cs`
6. `Application/UI/Components/GridComponent.cs`
7. `Application/UI/Pages/CollectionPage.cs`
8. `Application/UI/Pages/UsersPage.cs`
9. `Application/UI/Flows/UsersFlow.cs`

After those migrations:

- Remove `BaseTest.Executor`.
- Delete `ElementExecutor`.
- Delete `Waiter`.
- Delete `RetryHandler`.
- Delete `RetryPolicy`.
- Delete `RetryExceededException`.
- Delete `FrameworkException` if nothing else inherits from it.

## Do Not Do

- Do not create a replacement executor.
- Do not wrap every Playwright action in another helper.
- Do not migrate CRUD tests in the same phase as wrapper deletion.
- Do not add broad retries to hide flaky selectors or app state issues.
