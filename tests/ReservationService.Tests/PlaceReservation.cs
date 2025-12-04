using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Contracts.ReservationService;
using Contracts.AssetManagement;

namespace ReservationService.Tests;


public class PlaceReservation : TestEnvironment
{
    [Fact]
    public async Task GivenAValidReservationRequest_WhenIPlaceAReservation_TheResponseIsASuccess()
    {
        // Given a registered asset
        var assetRegistration = new AssetRegistrationRequest { Name = "Test Asset" };
        var assetResponse = await AssetManagementServiceClient.PostAsJsonAsync("/assets", assetRegistration);
        assetResponse.EnsureSuccessStatusCode();
        var asset = await assetResponse.Content.ReadFromJsonAsync<AssetRegistrationResponse>();
        Assert.NotNull(asset);
        var assetId = asset.Id;

        // And a valid reservation request
        var reservationRequest = new ReservationRequest
        {
            AssetId = assetId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        // When I place a reservation
        var response = await ReservationServiceClient.PostAsJsonAsync("/reservations", reservationRequest);

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
        // Given a reservation request with a non-existent asset ID
        var reservationRequest = new ReservationRequest
        {
            AssetId = "non-existent-asset-id",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };

        // When I place a reservation
        var response = await ReservationServiceClient.PostAsJsonAsync("/reservations", reservationRequest);

        // Then the response is an error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GivenAnExistingReservation_WhenITryToDoubleBookTheSameAsset_TheResponseIsAError()
    {
        // Given a registered asset
        var assetRegistration = new AssetRegistrationRequest { Name = "Test Asset" };
        var assetResponse = await AssetManagementServiceClient.PostAsJsonAsync("/assets", assetRegistration);
        assetResponse.EnsureSuccessStatusCode();
        var asset = await assetResponse.Content.ReadFromJsonAsync<AssetRegistrationResponse>();
        Assert.NotNull(asset);
        var assetId = asset.Id;

        // And an existing reservation
        var reservationRequest = new ReservationRequest
        {
            AssetId = assetId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        await ReservationServiceClient.PostAsJsonAsync("/reservations", reservationRequest);

        // When I try to double book the same asset
        var response = await ReservationServiceClient.PostAsJsonAsync("/reservations", reservationRequest);

        // Then the response is an error
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GivenAnExistingReservation_WhenIBookTheSameAssetWithNonOverlappingPeriod_TheReservationIsSuccessful()
    {
        // Given a registered asset
        var assetRegistration = new AssetRegistrationRequest { Name = "Test Asset" };
        var assetResponse = await AssetManagementServiceClient.PostAsJsonAsync("/assets", assetRegistration);
        assetResponse.EnsureSuccessStatusCode();
        var asset = await assetResponse.Content.ReadFromJsonAsync<AssetRegistrationResponse>();
        Assert.NotNull(asset);
        var assetId = asset.Id;

        // And an existing reservation
        var firstReservationRequest = new ReservationRequest
        {
            AssetId = assetId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1)
        };
        var firstResponse = await ReservationServiceClient.PostAsJsonAsync("/reservations", firstReservationRequest);
        firstResponse.EnsureSuccessStatusCode();

        // When I book the same asset with a non-overlapping period
        var secondReservationRequest = new ReservationRequest
        {
            AssetId = assetId,
            StartDate = DateTime.Now.AddDays(2),
            EndDate = DateTime.Now.AddDays(3)
        };
        var secondResponse = await ReservationServiceClient.PostAsJsonAsync("/reservations", secondReservationRequest);

        // Then the reservation is successful
        secondResponse.EnsureSuccessStatusCode();

        // And the response is a reservation
        var reservation = await secondResponse.Content.ReadFromJsonAsync<ReservationResponse>();
        Assert.NotNull(reservation);
        Assert.Equal(secondReservationRequest.AssetId, reservation.AssetId);
        Assert.Equal(secondReservationRequest.StartDate, reservation.StartDate);
        Assert.Equal(secondReservationRequest.EndDate, reservation.EndDate);
    }
}
