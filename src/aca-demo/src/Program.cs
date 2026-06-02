var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Service = "Orders API",
        Platform = "Azure Container Apps",
        Time = DateTime.UtcNow
    });
});

app.MapGet("/health", () => "Healthy");

app.MapPost("/orders", () =>
{
    return Results.Ok(new
    {
        Status = "Order Created"
    });
});

app.Run();