var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => "Ok");
app.MapPost("/assets", (AssetRegistrationRequest request) => new { Id = Guid.NewGuid() });

app.Run();

class AssetRegistrationRequest
{
  public required string Name { get; set; }
}

// Needed for the MVC test framework
public partial class Program { }
