namespace DataCaptureService.Contracts;

public interface IFileSystemWatcherService
{
    Task Run(CancellationToken stoppingToken);
}
