using AssetManagementService.Context;
using AssetManagementService.Services;
using Contracts.AssetManagement;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<AssetService>();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "asset_management";
var connectionString = $"Host={dbHost};Port={dbPort};Username={dbUser};Password={dbPassword};Database={dbName}";

builder.Services.AddDbContext<AssetManagementDbContext>(context =>
  context.UseNpgsql(connectionString));

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
