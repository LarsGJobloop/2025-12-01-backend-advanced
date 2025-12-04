using Contracts.ReservationService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var reservations = new Dictionary<string, ReservationResponse>();

app.MapGet("/health", () => "OK");

app.MapPost("/reservations", (ReservationRequest request) =>
{
  if (reservations.ContainsKey(request.AssetId))
  {
    return Results.BadRequest();
  }

  var newReservation = new ReservationResponse
  {
    Id = Guid.NewGuid(),
    AssetId = request.AssetId,
    StartDate = request.StartDate,
    EndDate = request.EndDate
  };

  reservations[request.AssetId] = newReservation;

  return Results.Ok(newReservation);
});

app.Run();

public partial class Program { }