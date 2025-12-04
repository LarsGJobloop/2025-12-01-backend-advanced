using Contracts.AssetManagement;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => "Ok");
app.MapPost("/assets", (AssetRegistrationRequest request) => new { Id = Guid.NewGuid() });

app.Run();

// Needed for the MVC test framework
public partial class Program { }
