using Microsoft.AspNetCore.Mvc.Testing;

public class AssetRegistration(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
  [Fact]
  public async Task GivenAnExistingAsset_WhenTheServiceIsRestarted_TheAssetStillExists()
  {
    // Given an existing asset

    // When the serices is restarted

    // Then the asset still exists
  }
}
