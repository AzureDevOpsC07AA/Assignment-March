using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShipPulse.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API base URL:
// - Local: http://localhost:5080
// - Dev/Prod: injected during build by workflow into wwwroot/appsettings.json
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5080";

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl.TrimEnd('/') + "/")
});

await builder.Build().RunAsync();
