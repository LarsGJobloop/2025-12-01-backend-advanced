using System.Net;
using System.Net.Http.Json;
using Contracts.AssetManagement;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AssetManagementService.Tests;

public class AssetListing : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory;

  public AssetListing(WebApplicationFactory<Program> factory)
  {
    _factory = factory;
  }

  [Fact]
  public async Task GivenNoAssets_WhenIListTheAssets_TheResponseIsAnEmptyList()
  {
    // Given no assets
    var client = _factory.CreateClient();

    // When I list the assets
    var response = await client.GetAsync("/assets");

    // Then the response is successful
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    // And the response is an empty list
    var assets = await response.Content.ReadFromJsonAsync<List<Asset>>();
    Assert.NotNull(assets);
    Assert.Empty(assets);
  }
}