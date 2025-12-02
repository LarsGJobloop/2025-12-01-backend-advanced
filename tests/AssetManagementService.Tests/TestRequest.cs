using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


namespace AssetManagementService.Tests;

public class TestRequst : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory;

  public TestRequst(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
  }

  [Fact]
  public void HelloTest()
  {
    // Create New Asset registration object

    // Make HTTP request against the API

    // Assert that it does what we expect
  }
}
