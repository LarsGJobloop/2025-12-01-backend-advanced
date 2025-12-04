namespace Contracts.ReservationService;

public class ReservationRequest
{
  public required string AssetId { get; init; }
  public required DateTime StartDate { get; init; }
  public required DateTime EndDate { get; init; }
}

public class ReservationResponse
{
  public required Guid Id { get; init; }
  public required string AssetId { get; init; }
  public required DateTime StartDate { get; init; }
  public required DateTime EndDate { get; init; }
}
