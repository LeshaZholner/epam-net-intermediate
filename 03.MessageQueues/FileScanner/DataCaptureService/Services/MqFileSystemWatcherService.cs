using DataCaptureService.Contracts;
using Microsoft.Extensions.Options;

namespace DataCaptureService;

public class MqFileSystemWatcherService : IFileSystemWatcherService
{
    private readonly FileSystemWatcherSettings _settings;
    private readonly IMessagingService _messagingService;

    public MqFileSystemWatcherService(IMessagingService messagingService, IOptions<FileSystemWatcherSettings> options)
    {
        _messagingService = messagingService;
        _settings = options.Value;
    }

    public async Task Run(CancellationToken stoppingToken)
    {
        using var sw = new FileSystemWatcher(_settings.Path, _settings.Filter)
        {
            NotifyFilter = NotifyFilters.LastAccess
                | NotifyFilters.LastWrite
                | NotifyFilters.FileName
                | NotifyFilters.CreationTime
        };

        sw.Created += (_, args) => FileHandler(args);
        sw.EnableRaisingEvents = true;

        var waitApp = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        await using var _ = stoppingToken.Register(() => waitApp.SetResult());
        await waitApp.Task;

        sw.EnableRaisingEvents = false;

    }

    private void FileHandler(FileSystemEventArgs args)
    {
        Task.Factory.StartNew(() =>
        {
            var fileInfo = new FileInfo(args.FullPath);
            if (fileInfo.IsFileLocked())
            {
                while (fileInfo.IsFileLocked())
                {
                    Thread.Sleep(_settings.Timeout);
                }
                SendFile(fileInfo);
            }
            else
            {
                SendFile(fileInfo);
            }
        });
    }

    private void SendFile(FileInfo fileInfo)
    {
        Task.Factory.StartNew(() =>
        {
            var fileName = BuildFileName(fileInfo.Name);
            var batch = new BatchFile(fileInfo.FullName, _settings.ChunkSize);
            
            foreach (var chunk in batch)
            {
                _messagingService.SendMessageAsync(new ChunkMessage
                {
                    FileName = fileName,
                    Extension = fileInfo.Extension,
                    Size = batch.Count,
                    Position = chunk.Position,
                    Data = chunk.Data
                });
            }
        });
    }

    private string BuildFileName(string fileName)
    {
        return $"{Path.GetFileNameWithoutExtension(fileName)}_{DateTime.Now.ToString("yyMMddhhmmss")}";
    }
}
