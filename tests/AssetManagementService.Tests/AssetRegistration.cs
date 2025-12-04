using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Contracts.AssetManagement;

namespace AssetManagementService.Tests;

public class AssetRegistration(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
  [Fact]
  public async Task GiveAValidRegistration_WhenICreateAnAsset_TheAssetIsCreated()
  {
    // Given a fresh client
    var client = Client;
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
    var client = Client;
    // And a valid registration
    var registration = new { Invalid = "Invalid" }; // This is an invalid registration

    // When I create the asset
    var response = await client.PostAsJsonAsync("/assets", registration);

    // Then the request is rejected
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  [Theory]
  [InlineData("Test Asset")]
  [InlineData("Test Asset 2")]
  [InlineData("Test Asset 3")]
  [InlineData("Test Asset 4")]
  [InlineData("Test Asset 5")]
  [InlineData("Test Asset 6")]
  [InlineData("Test Asset 7")]
  [InlineData("Test Asset 8")]
  [InlineData("Test Asset 9")]
  [InlineData("Test Asset 10")]
  public async Task GivenAnExistingAsset_WhenIRequestTheAsset_TheAssetIsReturned(string assetName)
  {
    // Given a registered asset
    var client = Client;
    var registration = new AssetRegistrationRequest { Name = assetName };
    var response = await client.PostAsJsonAsync("/assets", registration);
    var asset = await response.Content.ReadFromJsonAsync<AssetRegistrationResponse>();
    Assert.NotNull(asset);
    var assetId = asset.Id;

    // When I request the asset by ID
    var assetResponse = await client.GetFromJsonAsync<Asset>($"/assets/{assetId}");

    // Then the asset is returned
    Assert.NotNull(assetResponse);
    Assert.Equal(assetId, assetResponse.Id);
    Assert.Equal(registration.Name, assetResponse.Name);
  }

  [Fact]
  public async Task GivenAnNonExistingAsset_WhenIRequestTheAsset_TheRequestIsRejected()
  {
    // Given a fresh client
    var client = Client;
    // And a non existing asset ID
    var assetId = "non-existing-asset-id";

    // When I request the asset by ID
    var response = await client.GetAsync($"/assets/{assetId}");

    // Then the request is rejected
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }
}
