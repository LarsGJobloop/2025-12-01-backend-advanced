using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AssetManagementService.Tests;

public class HealthCheck(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
  /// <summary>
  /// A simple test to verify that we can actually connect to the server
  /// send messages, recive and parse responses.
  /// </summary>
  [Fact]
  public async Task WhenQuerrying_HealthEndpoint_ServerRespondsWithOK()
  {
    // Given a fresh client
    var client = Client;

    // When querry the Health Endpoint
    var response = await client.GetAsync("/health");

    // The response is a sucess
    response.EnsureSuccessStatusCode();
  }
}
