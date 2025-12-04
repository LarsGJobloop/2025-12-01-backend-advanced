using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ReservationService.Tests;

class ReservationRequest
{
    public required string AssetId { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
}

class ReservationResponse
{
    public required Guid Id { get; init; }
    public required string AssetId { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
}

public class PlaceReservation(WebApplicationFactory<Program> factory) : TestEnvironment(factory)
{
    [Fact]
    public async Task GivenAValidReservationRequest_WhenIPlaceAReservation_TheResponseIsASuccess()
    {
        // Given a valid reservation request
        var client = Client;
        var reservationRequest = new ReservationRequest
        {
            AssetId = "123",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        // When I place a reservation
        var response = await client.PostAsJsonAsync("/reservations", reservationRequest);

        // Then the response is successful
        response.EnsureSuccessStatusCode();

        // And the response is a reservation
        var reservation = await response.Content.ReadFromJsonAsync<ReservationResponse>();
        Assert.NotNull(reservation);
        Assert.Equal(reservationRequest.AssetId, reservation.AssetId);
        Assert.Equal(reservationRequest.StartDate, reservation.StartDate);
        Assert.Equal(reservationRequest.EndDate, reservation.EndDate);
    }
}
