namespace DataCaptureService;

public class FileSystemWatcherSettings
{
    public string Path { get; set; } = default!;
    public string Filter { get; set; } = default!;
    public int Timeout { get; set; }
    public int ChunkSize { get; set; }
}
