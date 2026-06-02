using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ShipPulse.Functions;

public class DailyReportFunction
{
    private readonly ILogger _logger;

    public DailyReportFunction(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<DailyReportFunction>();
    }

    [Function("DailyReportFunction")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("Generating scheduled logistics report");
    }
}