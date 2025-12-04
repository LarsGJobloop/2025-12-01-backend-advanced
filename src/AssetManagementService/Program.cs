using Contracts.AssetManagement;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => "Ok");
app.MapPost("/assets", (AssetRegistrationRequest request) => new { Id = Guid.NewGuid() });
app.MapGet("/assets/{id}", (string id) => new Asset { Id = id, Name = "Test Asset" });

app.Run();

// Needed for the MVC test framework
public partial class Program { }
