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
/// <param name="assetManagementServiceUrl">The URL of the Asset Management Service.</param>
public class AssetManagementServiceClient
{
  /// <summary>
  /// The HTTP client for the Asset Management Service.
  /// </summary>
  public HttpClient Client { get; }

  /// <summary>
  /// Initializes a new instance of the AssetManagementServiceClient class.
  /// </summary>
  /// <param name="assetManagementServiceUrl">The URL of the Asset Management Service.</param>
  public AssetManagementServiceClient(string assetManagementServiceUrl)
  {
    var client = new HttpClient();
    client.BaseAddress = new Uri(assetManagementServiceUrl);
    Client = client;
  }
}
