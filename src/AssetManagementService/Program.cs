using AssetManagementService.Context;
using AssetManagementService.Services;
using Contracts.AssetManagement;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<AssetService>();

builder.Services.AddDbContext<AssetManagementDbContext>();

var app = builder.Build();

app.MapGet("/health", () => "Ok");

app.MapPost("/assets", (
  AssetRegistrationRequest request,
  AssetService assetService
) =>
{
  var response = assetService.RegisterAsset(request);
  return Results.Ok(response);
});

app.MapGet("/assets/{id}", (string id, AssetService assetService) =>
{
  var asset = assetService.GetAsset(id);
  return asset is not null ? Results.Ok(asset) : Results.NotFound();
});

app.MapGet("/assets", (AssetService assetService) =>
{
  var assets = assetService.ListAssets();
  return Results.Ok(assets);
});

app.Run();

// Needed for the MVC test framework
public partial class Program { }
