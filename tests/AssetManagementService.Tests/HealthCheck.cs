using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AssetManagementService.Tests;

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
    // Given a fresh client
    var client = _factory.CreateClient();

    // When querry the Health Endpoint
    var response = await client.GetAsync("/health");

    // The response is a sucess
    response.EnsureSuccessStatusCode();
  }
}
