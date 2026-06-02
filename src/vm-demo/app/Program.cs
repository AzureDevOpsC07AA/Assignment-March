var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Service = "Legacy Inventory API",
        Environment = "Azure VM",
        Time = DateTime.UtcNow
    });
});

app.MapGet("/inventory", () =>
{
    return Results.Ok(new[]
    {
        new { Id = 1, Product = "Laptop", Stock = 20 },
        new { Id = 2, Product = "Mouse", Stock = 50 }
    });
});

app.Run();