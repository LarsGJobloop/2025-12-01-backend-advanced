using Contracts.AssetManagement;

namespace AssetManagementService.Services;

/// <summary>
/// Service for managing assets.
/// </summary>
public class AssetService
{
  private readonly Dictionary<string, Asset> _assets = new();

  /// <summary>
  /// Registers a new asset.
  /// </summary>
  /// <param name="request">The asset registration request.</param>
  /// <returns>The asset registration response with the generated asset ID.</returns>
  public AssetRegistrationResponse RegisterAsset(AssetRegistrationRequest request)
  {
    var asset = new Asset { Id = Guid.NewGuid().ToString(), Name = request.Name };
    _assets.Add(asset.Id, asset);
    return new AssetRegistrationResponse { Id = asset.Id };
  }

  /// <summary>
  /// Retrieves an asset by its ID.
  /// </summary>
  /// <param name="id">The asset ID.</param>
  /// <returns>The asset if found, null otherwise.</returns>
  public Asset? GetAsset(string id)
  {
    _assets.TryGetValue(id, out var asset);
    return asset;
  }

  /// <summary>
  /// Lists all assets.
  /// </summary>
  /// <returns>A list of all assets.</returns>
  public List<Asset> ListAssets()
  {
    return _assets.Values.ToList();
  }
}

