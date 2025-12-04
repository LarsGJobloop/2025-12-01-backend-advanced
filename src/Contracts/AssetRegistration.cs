namespace Contracts.AssetManagement;

public class AssetRegistrationRequest
{
  public required string Name { get; set; }
}

public class AssetRegistrationResponse
{
  public required string Id { get; set; }
}
