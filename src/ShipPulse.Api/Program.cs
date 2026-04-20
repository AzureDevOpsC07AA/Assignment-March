using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Basic config
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShipPulse API", Version = "v1" });
});

// CORS for local + static web apps (set allowed origins via config)
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
        }
        else
        {
            // Safe default for local learning
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    });
});

// In-memory store (startup MVP). In real prod, swap to DB.
builder.Services.AddSingleton<FeedbackStore>();

var app = builder.Build();

app.UseCors();

// Only enable Swagger in Development to be closer to real-world practices
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", (IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        status = "ok",
        environment = env.EnvironmentName,
        timeUtc = DateTime.UtcNow
    });
});

app.MapGet("/api/feedback", (FeedbackStore store) =>
{
    return Results.Ok(store.GetAll());
});


app.MapGet("/", (IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        status = "ok",
        environment = env.EnvironmentName,
        timeUtc = DateTime.UtcNow
    });
});

app.MapGet("/hello", (IHostEnvironment env) =>
{
    return Results.Ok(new
    {
        status = "ok",
        environment = env.EnvironmentName,
        timeUtc = DateTime.UtcNow
    });
});

app.MapPost("/api/feedback", (CreateFeedbackRequest req, FeedbackStore store) =>
{
    if (string.IsNullOrWhiteSpace(req.Message) || req.Message.Length < 3)
        return Results.BadRequest(new { error = "Message must be at least 3 characters." });

    var item = store.Add(req.Message.Trim(), req.CreatedBy?.Trim());
    return Results.Created($"/api/feedback/{item.Id}", item);
});

app.Run();

public record CreateFeedbackRequest(string Message, string? CreatedBy);

public record FeedbackItem(
    string Id,
    string Message,
    string? CreatedBy,
    DateTime CreatedAtUtc
);

public sealed class FeedbackStore
{
    private readonly List<FeedbackItem> _items = new();

    public IReadOnlyList<FeedbackItem> GetAll()
        => _items.OrderByDescending(x => x.CreatedAtUtc).ToList();

    public FeedbackItem Add(string message, string? createdBy)
    {
        var item = new FeedbackItem(
            Id: Guid.NewGuid().ToString("n"),
            Message: message,
            CreatedBy: string.IsNullOrWhiteSpace(createdBy) ? "anonymous" : createdBy,
            CreatedAtUtc: DateTime.UtcNow
        );

        _items.Add(item);
        return item;
    }
}


public partial class Program { }
