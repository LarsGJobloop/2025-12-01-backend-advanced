namespace ReservationService.Services;

/// <summary>
/// Client for the Asset Management Service.
/// </summary>
/// <remarks>
/// This class uses a dedicated HttpClient instance for interacting with the Asset Management Service.
/// By exposing the HttpClient as a public property, we enable testability: during testing, the HttpClient
/// can be replaced via dependency injection using AddHttpClient and ConfigurePrimaryHttpMessageHandler
/// (as demonstrated in TestEnvironment.cs). This allows tests to inject a test server's message handler,
/// enabling the ReservationService to make HTTP calls to an in-memory test instance of the Asset Management
/// Service instead of requiring the actual service to be running. This approach allows tests to run in
/// isolation and verify behavior without depending on external services being available.
/// </remarks>
public class AssetManagementServiceClient
{
  /// <summary>
  /// The HTTP client for the Asset Management Service.
  /// </summary>
  public HttpClient Client { get; }

  /// <summary>
  /// Initializes a new instance of the AssetManagementServiceClient class using dependency injection.
  /// This constructor is used when the client is registered via AddHttpClient.
  /// </summary>
  /// <param name="httpClient">The HTTP client instance injected via dependency injection.</param>
  public AssetManagementServiceClient(HttpClient httpClient)
  {
    Client = httpClient;
  }
}
