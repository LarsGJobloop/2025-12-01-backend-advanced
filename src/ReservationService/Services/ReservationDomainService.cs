using Contracts.ReservationService;

namespace ReservationService.Services;

/// <summary>
/// Domain service for managing reservations.
/// </summary>
public class ReservationDomainService
{
  private readonly Dictionary<string, List<ReservationResponse>> _reservations = new();

  /// <summary>
  /// Checks if a reservation period overlaps with existing reservations for an asset.
  /// </summary>
  /// <param name="assetId">The asset ID.</param>
  /// <param name="startDate">The start date of the reservation period.</param>
  /// <param name="endDate">The end date of the reservation period.</param>
  /// <returns>True if there's an overlap, false otherwise.</returns>
  public bool HasOverlappingReservation(string assetId, DateTime startDate, DateTime endDate)
  {
    if (!_reservations.TryGetValue(assetId, out var existingReservations))
    {
      return false;
    }

    return existingReservations.Any(existing =>
      startDate < existing.EndDate && endDate > existing.StartDate);
  }

  /// <summary>
  /// Creates a new reservation.
  /// </summary>
  /// <param name="request">The reservation request.</param>
  /// <returns>The created reservation response.</returns>
  public ReservationResponse CreateReservation(ReservationRequest request)
  {
    var newReservation = new ReservationResponse
    {
      Id = Guid.NewGuid(),
      AssetId = request.AssetId,
      StartDate = request.StartDate,
      EndDate = request.EndDate
    };

    if (!_reservations.ContainsKey(request.AssetId))
    {
      _reservations[request.AssetId] = new List<ReservationResponse>();
    }
    _reservations[request.AssetId].Add(newReservation);

    return newReservation;
  }
}

