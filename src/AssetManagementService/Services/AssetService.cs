using AssetManagementService.Context;
using Contracts.AssetManagement;

namespace AssetManagementService.Services;

/// <summary>
/// Service for managing assets.
/// </summary>
public class AssetService
{
  private readonly AssetManagementDbContext _context;

  public AssetService(AssetManagementDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Registers a new asset.
  /// </summary>
  /// <param name="request">The asset registration request.</param>
  /// <returns>The asset registration response with the generated asset ID.</returns>
  public AssetRegistrationResponse RegisterAsset(AssetRegistrationRequest request)
  {
    var asset = new Asset { Id = Guid.NewGuid().ToString(), Name = request.Name };
    _context.Assets.Add(asset);
    _context.SaveChanges();
    return new AssetRegistrationResponse { Id = asset.Id };
  }

  /// <summary>
  /// Retrieves an asset by its ID.
  /// </summary>
  /// <param name="id">The asset ID.</param>
  /// <returns>The asset if found, null otherwise.</returns>
  public Asset? GetAsset(string id)
  {
    return _context.Assets.FirstOrDefault(a => a.Id == id);
  }

  /// <summary>
  /// Lists all assets.
  /// </summary>
  /// <returns>A list of all assets.</returns>
  public List<Asset> ListAssets()
  {
    return _context.Assets.ToList();
  }
}

