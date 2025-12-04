using Microsoft.AspNetCore.Mvc.Testing;

/// <summary>
/// Integration test environment for the Reservation Service.
/// 
/// Provides an in-memory test server using WebApplicationFactory and xUnit's IClassFixture
/// to share the test server instance across all tests in a class.
/// 
/// Usage:
///   public class MyTests : TestEnvironment
///   {
///     public MyTests(WebApplicationFactory<Program> factory) : base(factory) { }
///     
///     [Fact]
///     public async Task MyTest()
///     {
///       var response = await Client.GetAsync("/api/endpoint");
///       // Assert on response...
///     }
///   }
/// </summary>
public class TestEnvironment : IClassFixture<WebApplicationFactory<Program>>
{
  /// <summary>
  /// The factory that creates an in-memory test server.
  /// </summary>
  protected readonly WebApplicationFactory<Program> _factory;

  /// <summary>
  /// Initializes a new instance of the TestEnvironment class.
  /// </summary>
  public TestEnvironment(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
  }

  /// <summary>
  /// HTTP client for sending requests to the test server.
  /// </summary>
  public HttpClient Client => _factory.CreateClient();
}
