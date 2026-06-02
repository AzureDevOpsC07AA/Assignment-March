using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ShipPulse.Functions;

public class OrderNotificationFunction
{
    private readonly ILogger _logger;

    public OrderNotificationFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<OrderNotificationFunction>();
    }

    [Function("OrderNotificationFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")]
        HttpRequestData req)
    {
        _logger.LogInformation("Processing notification request");

        var response = req.CreateResponse(HttpStatusCode.OK);

        await response.WriteStringAsync("Notification sent successfully");

        return response;
    }
}