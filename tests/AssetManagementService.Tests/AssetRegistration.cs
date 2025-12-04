using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Contracts.AssetManagement;

namespace AssetManagementService.Tests;

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

  [Fact]
  public async Task GivenAnInvalidRegistration_WhenICreateAnAsset_TheRequestIsRejected()
  {
    // Given a fresh client
    var client = _factory.CreateClient();
    // And a valid registration
    var registration = new { Invalid = "Invalid" }; // This is an invalid registration

    // When I create the asset
    var response = await client.PostAsJsonAsync("/assets", registration);

    // Then the request is rejected
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Fact]
  public async Task GivenAnExistingAsset_WhenIRequestTheAsset_TheAssetIsReturned()
  {
    // Given a registered asset
    var client = _factory.CreateClient();
    var registration = new AssetRegistrationRequest { Name = "Test Asset" };
    var response = await client.PostAsJsonAsync("/assets", registration);
    var asset = await response.Content.ReadFromJsonAsync<AssetRegistrationResponse>();
    Assert.NotNull(asset);
    var assetId = asset.Id;

    // When I request the asset by ID
    var assetResponse = await client.GetFromJsonAsync<AssetRegistrationResponse>($"/assets/{assetId}");
    Assert.NotNull(assetResponse);
    Assert.Equal(assetId, assetResponse.Id);
    Assert.Equal(registration.Name, assetResponse.Name);
  }
}
