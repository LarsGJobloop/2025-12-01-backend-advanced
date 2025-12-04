using Microsoft.AspNetCore.Mvc.Testing;

namespace ReservationService.Tests;

public class HealthCheck : TestEnvironment
{
  /// <summary>
  /// A simple test to verify that we can actually connect to the server
  /// send messages, recive and parse responses.
  /// </summary>
  [Fact]
  public async Task WhenQuerrying_HealthEndpoint_ServerRespondsWithOK()
  {
    // When querry the Health Endpoint
    var response = await ReservationServiceClient.GetAsync("/health");

    // The response is a sucess
    response.EnsureSuccessStatusCode();
  }
}
