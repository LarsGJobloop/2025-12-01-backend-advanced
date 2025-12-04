using Contracts.ReservationService;
using Contracts.AssetManagement;
using ReservationService.Services;

var builder = WebApplication.CreateBuilder(args);

// Register AssetManagementServiceClient
// Hardcoding for now as we have no test that says otherwise.
var assetManagementServiceUrl = "http://localhost:5002";
builder.Services.AddHttpClient<AssetManagementServiceClient>(client =>
{
  client.BaseAddress = new Uri(assetManagementServiceUrl);
});

var app = builder.Build();

var reservations = new Dictionary<string, List<ReservationResponse>>();

app.MapGet("/health", () => "OK");

app.MapPost("/reservations", async (ReservationRequest request, AssetManagementServiceClient assetClient) =>
{
  // Validate that the asset exists in the Asset Management Service
  var assetResponse = await assetClient.Client.GetAsync($"/assets/{request.AssetId}");
  if (assetResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
  {
    return Results.BadRequest();
  }

  // Check for overlapping reservations
  if (reservations.TryGetValue(request.AssetId, out var existingReservations))
  {
    var hasOverlap = existingReservations.Any(existing =>
      request.StartDate < existing.EndDate && request.EndDate > existing.StartDate);

    if (hasOverlap)
    {
      return Results.BadRequest();
    }
  }

  // Create a new reservation
  var newReservation = new ReservationResponse
  {
    Id = Guid.NewGuid(),
    AssetId = request.AssetId,
    StartDate = request.StartDate,
    EndDate = request.EndDate
  };

  // Persist the reservation
  if (!reservations.ContainsKey(request.AssetId))
  {
    reservations[request.AssetId] = new List<ReservationResponse>();
  }
  reservations[request.AssetId].Add(newReservation);

  // Return the reservation
  return Results.Ok(newReservation);
});

app.Run();

public partial class Program { }