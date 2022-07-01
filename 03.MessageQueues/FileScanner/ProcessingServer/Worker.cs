using ProcessingService.Messages;

namespace ProcessingService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMessagesBroker _messagesBroker;

    public Worker(ILogger<Worker> logger, IMessagesBroker messagesBroker)
    {
        _logger = logger;
        _messagesBroker = messagesBroker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _messagesBroker.Run<FileChunk>(stoppingToken);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Message broker failed. {ErrorMessage}", e.Message);
        }
        
    }
}