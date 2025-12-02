var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => "Ok");

app.Run();

// Needed for the MVC test framework
public partial class Program { }
