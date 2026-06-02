using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ShipPulse.Functions;

public class ShipmentQueueFunction
{
    private readonly ILogger _logger;

    public ShipmentQueueFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ShipmentQueueFunction>();
    }

    [Function("ShipmentQueueFunction")]
    public void Run(
        [QueueTrigger("shipment-events", Connection = "AzureWebJobsStorage")]
        string queueMessage)
    {
        _logger.LogInformation($"Shipment event received: {queueMessage}");

        // Simulate sending notification
        _logger.LogInformation("Customer notification sent");
    }
}