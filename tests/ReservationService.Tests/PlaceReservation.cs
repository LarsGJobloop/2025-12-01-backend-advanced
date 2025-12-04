using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Contracts.ReservationService;

namespace ReservationService.Tests;


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

    [Fact]
    public async Task GivenAnInvalidReservationRequest_WhenIPlaceAReservation_TheResponseIsAError()
    {
        // Given an invalid reservation request
        var client = Client;
        var reservationRequest = new { Invalid = "Invalid" };

        // When I place a reservation
        var response = await client.PostAsJsonAsync("/reservations", reservationRequest);

        // Then the response is an error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GivenAnExistingReservation_WhenITryToDoubleBookTheSameAsset_TheResponseIsAError()
    {
        // Given an existing reservation
        var client = Client;
        var reservationRequest = new ReservationRequest
        {
            AssetId = "123",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        await client.PostAsJsonAsync("/reservations", reservationRequest);

        // When I try to double book the same asset
        var response = await client.PostAsJsonAsync("/reservations", reservationRequest);

        // Then the response is an error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
