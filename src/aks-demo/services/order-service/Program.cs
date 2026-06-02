var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        Service = "Order Service",
        Platform = "AKS"
    });
});

app.Run();