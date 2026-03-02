using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ShipPulse.Tests;

public class HealthTests
{
    [Fact]
    public async Task Health_ReturnsOk()
    {
        await using var app = new WebApplicationFactory<Program>();
        var client = app.CreateClient();
        var resp = await client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }
}
