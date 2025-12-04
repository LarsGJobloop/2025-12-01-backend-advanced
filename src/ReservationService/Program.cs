using Contracts.ReservationService;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => "OK");

app.MapPost("/reservations", (ReservationRequest request) =>
{
  return Results.Ok(new ReservationResponse
  {
    Id = Guid.NewGuid(),
    AssetId = request.AssetId,
    StartDate = request.StartDate,
    EndDate = request.EndDate
  });
});

app.Run();

public partial class Program { }