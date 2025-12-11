using System.Net.Http.Json;
using Contracts.AssetManagement;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AssetManagementService.Tests;

public class DurabilityRequirement(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
  [Fact]
  public async Task GivenAnExistingAsset_WhenTheServiceIsRestarted_TheAssetStillExists()
  {
    // Given an existing asset
    var client = Client;
    var registration = new AssetRegistrationRequest { Name = "Test Asset" };
    var response = await client.PostAsJsonAsync("/assets", registration);
    var asset = await response.Content.ReadFromJsonAsync<AssetRegistrationResponse>();
    Assert.NotNull(asset);
    var assetId = asset.Id;

    // When the serices is restarted
    await RestartAsync();
    client = Client;

    // Then the asset is still available
    var assetResponse = await client.GetFromJsonAsync<Asset>($"/assets/{assetId}");
    Assert.NotNull(assetResponse);
    Assert.Equal(assetId, assetResponse.Id);
    Assert.Equal(registration.Name, assetResponse.Name);
  }
}
