/// <summary>
/// A test environment for the Reservation Service that includes both the Reservation Service and
/// the Asset Management Service running as in-memory test servers.
/// 
/// INTEGRATION TESTING WITH MULTIPLE SERVICES:
/// Unlike unit tests that mock dependencies, this test environment runs both services as real
/// in-memory servers. This approach:
/// - Tests the actual HTTP communication between services
/// - Validates that contracts (request/response formats) match between services
/// - Catches integration issues that mocks might miss
/// - Is more realistic than mocking, though slightly slower
/// 
/// WHAT IS extern alias?
/// The `extern alias` keyword allows us to reference two different assemblies that have types with
/// the same name. In this case, both ReservationService and AssetManagementService have a `Program`
/// class. By using `extern alias AssetManagementService`, we can reference the AssetManagementService's
/// Program class without conflicts. The alias `AssetManagmentServiceProgram` gives us a convenient
/// way to refer to it.
/// 
/// HOW THIS WORKS:
/// 1. We create two WebApplicationFactory instances - one for each service
/// 2. The AssetManagementService factory creates an in-memory test server for that service
/// 3. The ReservationService factory is configured to use dependency injection to replace the
///    HttpClient used by AssetManagementServiceClient
/// 4. When ReservationService makes HTTP calls to AssetManagementService, they go to the
///    in-memory test server instead of a real network endpoint
/// 
/// HTTP CLIENT REPLACEMENT VIA DEPENDENCY INJECTION:
/// The key to making this work is ConfigurePrimaryHttpMessageHandler. This method tells the
/// dependency injection container: "When someone requests an HttpClient for AssetManagementServiceClient,
/// use this custom message handler instead of the default one." The custom handler is created by
/// calling CreateHandler() on the AssetManagementService test server, which routes all HTTP requests
/// to that in-memory server.
/// 
/// WHY NOT JUST MOCK?
/// While mocking would be faster, using real test servers ensures:
/// - The actual HTTP pipeline is tested (routing, middleware, serialization)
/// - Contract compatibility is verified (if the services don't match, tests fail)
/// - More confidence that the services will work together in production
/// 
/// LIFECYCLE:
/// This class implements IAsyncDisposable to properly clean up resources. When tests complete,
/// both test servers are disposed, releasing any resources they were using.
/// 
/// HOW TO USE THIS CLASS - CONCRETE EXAMPLE:
/// To create a new test specification, inherit from TestEnvironment and use the provided HTTP clients:
/// 
///   public class MyNewSpecification : TestEnvironment
///   {
///     [Fact]
///     public async Task GivenSomeCondition_WhenIAction_ThenExpectedResult()
///     {
///       // If you need to set up data in Asset Management Service first:
///       var assetRequest = new AssetRegistration { /* ... */ };
///       await AssetManagementServiceClient.PostAsJsonAsync("/assets", assetRequest);
///       
///       // Then test the Reservation Service:
///       var reservationRequest = new ReservationRequest { /* ... */ };
///       var response = await ReservationServiceClient.PostAsJsonAsync("/reservations", reservationRequest);
///       
///       // Assert on the response
///       response.EnsureSuccessStatusCode();
///       var reservation = await response.Content.ReadFromJsonAsync<ReservationResponse>();
///       Assert.NotNull(reservation);
///     }
///   }
/// 
/// The TestEnvironment base class handles all the setup and teardown automatically. You just need to:
/// 1. Inherit from TestEnvironment
/// 2. Use ReservationServiceClient to test the Reservation Service
/// 3. Use AssetManagementServiceClient to set up test data in the Asset Management Service
/// 4. Write your test logic using standard xUnit patterns
/// </summary>
extern alias AssetManagementService;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using ReservationService;
using ReservationService.Services;
using AssetManagmentServiceProgram = AssetManagementService::Program;

public class TestEnvironment : IAsyncDisposable
{
  /// <summary>
  /// Factory that creates an in-memory test server for the Reservation Service.
  /// </summary>
  private readonly WebApplicationFactory<Program> _reservationServiceClientFactory;

  /// <summary>
  /// Factory that creates an in-memory test server for the Asset Management Service.
  /// </summary>
  private readonly WebApplicationFactory<AssetManagmentServiceProgram> _assetManagementServiceClientFactory;

  /// <summary>
  /// Initializes a new test environment with both services running as in-memory test servers.
  /// 
  /// The constructor:
  /// 1. Creates the AssetManagementService test server first
  /// 2. Creates the ReservationService test server and configures it to route HTTP calls
  ///    from AssetManagementServiceClient to the AssetManagementService test server
  /// </summary>
  public TestEnvironment()
  {
    // Create the Asset Management Service test server
    _assetManagementServiceClientFactory = new WebApplicationFactory<AssetManagmentServiceProgram>();

    // Create the Reservation Service test server and configure it to use the Asset Management Service test server
    _reservationServiceClientFactory = new WebApplicationFactory<Program>()
      .WithWebHostBuilder(builder =>
      {
        builder.ConfigureServices(services =>
        {
          // Register AssetManagementServiceClient with dependency injection
          // ConfigurePrimaryHttpMessageHandler replaces the default HTTP handler with one that
          // routes requests to our AssetManagementService test server
          services.AddHttpClient<AssetManagementServiceClient>()
            .ConfigurePrimaryHttpMessageHandler(() => _assetManagementServiceClientFactory.Server.CreateHandler());
        });
      });
  }

  /// <summary>
  /// HTTP client for making requests to the Reservation Service test server.
  /// </summary>
  public HttpClient ReservationServiceClient => _reservationServiceClientFactory.CreateClient();

  /// <summary>
  /// HTTP client for making requests to the Asset Management Service test server.
  /// 
  /// This is useful when you need to set up test data (e.g., register an asset) before
  /// testing the Reservation Service's behavior.
  /// </summary>
  public HttpClient AssetManagementServiceClient => _assetManagementServiceClientFactory.CreateClient();

  /// <summary>
  /// Disposes of the test servers and releases resources.
  /// </summary>
  public async ValueTask DisposeAsync()
  {
    await _reservationServiceClientFactory.DisposeAsync();
    await _assetManagementServiceClientFactory.DisposeAsync();
  }
}
