namespace Contracts.AssetManagement;

public class Asset
{
  public required string Id { get; set; }
  public required string Name { get; set; }
}

public class AssetRegistrationRequest
{
  public required string Name { get; set; }
}

public class AssetRegistrationResponse
{
  public required string Id { get; set; }
}
