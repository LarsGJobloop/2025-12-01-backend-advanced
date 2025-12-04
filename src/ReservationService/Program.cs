using Contracts.ReservationService;
using ReservationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Register AssetManagementServiceClient
// Hardcoding for now as we have no test that says otherwise.
var assetManagementServiceUrl = "http://localhost:5002";
builder.Services.AddHttpClient<AssetManagementServiceClient>(client =>
{
  client.BaseAddress = new Uri(assetManagementServiceUrl);
});

// Register domain services
builder.Services.AddSingleton<ReservationDomainService>();

var app = builder.Build();

app.MapGet("/health", () => "OK");

app.MapPost("/reservations", async (
  ReservationRequest request,
  AssetManagementServiceClient assetClient,
  ReservationDomainService reservationDomainService
) =>
{
  // Validate that the asset exists in the Asset Management Service
  var assetResponse = await assetClient.Client.GetAsync($"/assets/{request.AssetId}");
  if (assetResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
  {
    return Results.BadRequest();
  }

  // Check for overlapping reservations
  if (reservationDomainService.HasOverlappingReservation(request.AssetId, request.StartDate, request.EndDate))
  {
    return Results.BadRequest();
  }

  // Create a new reservation
  var newReservation = reservationDomainService.CreateReservation(request);

  // Return the reservation
  return Results.Ok(newReservation);
});

app.Run();

public partial class Program { }