using DataCaptureService.Contracts;

namespace DataCaptureService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IFileSystemWatcherService _fileSystemWatcherService;
    
    public Worker(ILogger<Worker> logger, IFileSystemWatcherService fileSystemWatcherService)
    {
        _logger = logger;
        _fileSystemWatcherService = fileSystemWatcherService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _fileSystemWatcherService.Run(stoppingToken);
    }
}