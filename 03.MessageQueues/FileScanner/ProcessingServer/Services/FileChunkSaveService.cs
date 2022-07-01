using Microsoft.Extensions.Options;
using ProcessingService.Settings;
using ProcessingService.Messages;
using ProcessingService.Contacts;

namespace ProcessingService.Services;

public class FileChunkSaveService : ISaveService<FileChunk>
{
    private readonly ILogger<FileChunkSaveService> _logger;
    private readonly FileChunkSaveSettings _settings;

    public FileChunkSaveService(ILogger<FileChunkSaveService> logger, IOptions<FileChunkSaveSettings> options)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task<bool> SaveMessageAsync(FileChunk message)
    {
        try
        {
            using var stream = new FileStream($"{_settings.Path}\\{message.FileName}{message.Extension}", FileMode.Append);
            await stream.WriteAsync(message.Data, 0, message.Data.Length);

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save File Chunk '{fileName}':'{position}'", message.FileName, message.Position);
            return false;
        }
    }
}
