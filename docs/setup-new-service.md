# Setup New Service

## 1. Create Service Project

```sh
SERVICE_NAME=YourServiceName
dotnet new webapi --name $SERVICE_NAME --output src/$SERVICE_NAME
dotnet add src/$SERVICE_NAME reference src/Contracts
dotnet sln add src/$SERVICE_NAME
```

## 2. Create Test Project

```sh
dotnet new xunit --name $SERVICE_NAME.Tests --output tests/$SERVICE_NAME.Tests
dotnet add tests/$SERVICE_NAME.Tests package Microsoft.AspNetCore.Mvc.Testing
dotnet add tests/$SERVICE_NAME.Tests reference src/$SERVICE_NAME
dotnet add tests/$SERVICE_NAME.Tests reference src/Contracts
dotnet sln add tests/$SERVICE_NAME.Tests
```

## 3. Add Health Check

Add to `src/$SERVICE_NAME/Program.cs`:

```csharp
app.MapGet("/health", () => "OK");
```

Create `tests/$SERVICE_NAME.Tests/HealthCheck.cs`:

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using YourServiceName;

namespace YourServiceName.Tests;

public class HealthCheck : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory;

  public HealthCheck(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task WhenQuerrying_HealthEndpoint_ServerRespondsWithOK()
  {
    var client = _factory.CreateClient();
    var response = await client.GetAsync("/health");
    response.EnsureSuccessStatusCode();
  }
}
```

## 4. Verify

```sh
dotnet test
```

> [!NOTE]
> For reusable test infrastructure, see [tests/AssetManagementService.Tests/TestEnvironment.cs](tests/AssetManagementService.Tests/TestEnvironment.cs). For multi-service integration testing, see [tests/ReservationService.Tests/TestEnvironment.cs](tests/ReservationService.Tests/TestEnvironment.cs).
