using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AssetManagementService.Tests;

class AssetRegistrationRequest
{
  public required string Name { get; set; }
}

class AssetRegistrationResponse
{
  public required string Id { get; set; }
}

public class AssetRegistration : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory;

  public AssetRegistration(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
  }


  [Fact]
  public async Task GiveAValidRegistration_WhenICreateAnAsset_TheAssetIsCreated()
  {
    // Given a fresh client
    var client = _factory.CreateClient();
    // And a valid registration
    var registration = new AssetRegistrationRequest { Name = "Test Asset" };

    // When I create the asset
    var response = await client.PostAsJsonAsync("/assets", registration);
    response.EnsureSuccessStatusCode();

    // Then the asset ID is returned
    var createResponse = await response.Content.ReadFromJsonAsync<AssetRegistrationResponse>();
    Assert.NotNull(createResponse);
    Assert.NotEmpty(createResponse.Id);
  }
}
