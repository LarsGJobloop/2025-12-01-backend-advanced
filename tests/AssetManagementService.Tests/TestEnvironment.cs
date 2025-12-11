using Microsoft.AspNetCore.Mvc.Testing;

/// <summary>
/// A test environment for the Asset Management Service.
/// 
/// This class provides an integration testing environment that allows you to test your ASP.NET Core
/// application end-to-end without needing to deploy it or start a real web server. It's a powerful
/// abstraction that bridges the gap between unit tests (which test individual components in isolation)
/// and manual testing (which requires running the full application).
/// 
/// INTEGRATION TESTING vs UNIT TESTING:
/// - Unit tests: Test individual methods/classes in isolation, often with mocked dependencies
/// - Integration tests: Test how multiple components work together, including HTTP requests/responses,
///   routing, middleware, and the full request pipeline
/// 
/// WHAT IS WebApplicationFactory?
/// WebApplicationFactory is a class provided by ASP.NET Core that creates an in-memory test server.
/// Think of it as a "virtual web server" that runs your application in memory during tests. It:
/// - Starts your application (the Program class) in a test environment
/// - Creates an HTTP client that can send requests to your application
/// - Handles all the HTTP pipeline (routing, middleware, controllers, etc.)
/// - Doesn't require network ports or external dependencies
/// 
/// WHAT IS IClassFixture?
/// IClassFixture is an xUnit interface that enables "fixture sharing" - a way to share setup
/// code across multiple test methods in the same test class. When you implement IClassFixture<T>:
/// - xUnit creates ONE instance of T (in this case, WebApplicationFactory) for the entire test class
/// - That same instance is injected into the constructor of each test method
/// - The factory is disposed after all tests in the class complete
/// 
/// LIFECYCLE EXAMPLE:
/// If you have 10 test methods in a class using TestEnvironment:
/// 1. xUnit creates ONE WebApplicationFactory instance
/// 2. That factory is injected into the TestEnvironment constructor
/// 3. All 10 tests share the same factory instance
/// 4. Each test calls _factory.CreateClient() to get an HTTP client
/// 5. After all tests complete, the factory is disposed
/// 
/// HOW TO USE THIS CLASS:
/// In your test classes, inherit from TestEnvironment and use _factory.CreateClient() to get an
/// HTTP client that can send requests to your application:
/// 
///   public class MyTests : TestEnvironment
///   {
///     public MyTests(WebApplicationFactory<Program> factory) : base(factory) { }
///     
///     [Fact]
///     public async Task MyTest()
///     {
///       var client = _factory.CreateClient();
///       var response = await client.GetAsync("/api/endpoint");
///       // Assert on response...
///     }
///   }
/// 
/// WHY THIS ABSTRACTION MATTERS:
/// Without this pattern, each test class would need to:
/// - Implement IClassFixture<WebApplicationFactory<Program>> itself
/// - Store the factory as a field
/// - Handle the constructor injection
/// 
/// By creating TestEnvironment, we:
/// - Encapsulate this common pattern
/// - Make it easier to add shared test setup/teardown logic later
/// - Provide a consistent base class for all integration tests
/// - Reduce boilerplate code in test classes
/// </summary>
public class TestEnvironment : IClassFixture<WebApplicationFactory<Program>>
{
  /// <summary>
  /// The factory that creates an in-memory test server for our application.
  /// 
  /// This field stores the WebApplicationFactory instance that was created by xUnit and injected
  /// into our constructor. We mark it as 'readonly' because once it's set in the constructor,
  /// it should never change during the lifetime of the test class.
  /// 
  /// The factory is shared across all test methods in the class, so all tests use the same
  /// test server instance.
  /// </summary>
  protected WebApplicationFactory<Program> _factory;

  /// <summary>
  /// Constructor that receives the WebApplicationFactory instance from xUnit.
  /// 
  /// This constructor is called by xUnit's test framework when it creates an instance of
  /// TestEnvironment. The framework automatically:
  /// 1. Creates a WebApplicationFactory<Program> instance (if one doesn't already exist for the class)
  /// 2. Passes it to this constructor
  /// 3. Stores it in the _factory field for use in test methods
  /// 
  /// The 'Program' type parameter tells the factory which application to start. It looks for
  /// the entry point of your application (usually Program.cs) and uses it to configure the test server.
  /// </summary>
  /// <param name="factory">
  /// The WebApplicationFactory instance created by xUnit. This will be the same instance
  /// shared across all test methods in classes that inherit from TestEnvironment.
  /// </param>
  public TestEnvironment(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
  }

  /// <summary>
  /// The HTTP client that can send requests to the test server.
  /// </summary>
  public HttpClient Client => _factory.CreateClient();

  /// <summary>
  /// Restart the test server.
  /// </summary>
  /// <returns></returns>
  public async Task RestartAsync()
  {
    await _factory.DisposeAsync();
    _factory = new WebApplicationFactory<Program>();
  }
}
