using Contracts.AssetManagement;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var assets = new Dictionary<string, Asset>();

app.MapGet("/health", () => "Ok");

app.MapPost("/assets", (AssetRegistrationRequest request) =>
{
  var asset = new Asset { Id = Guid.NewGuid().ToString(), Name = request.Name };
  assets.Add(asset.Id, asset);
  var response = new AssetRegistrationResponse { Id = asset.Id };
  return Results.Ok(response);
});

app.MapGet("/assets/{id}", (string id) =>
{
  assets.TryGetValue(id, out var asset);
  return asset is not null ? Results.Ok(asset) : Results.NotFound();
});

app.Run();

// Needed for the MVC test framework
public partial class Program { }
