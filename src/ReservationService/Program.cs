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

public partial class Program { }