namespace ProcessingService.Messages;

public class FileChunk
{
    public string FileName { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public int Size { get; set; }
    public int Position { get; set; }
    public byte[] Data { get; set; } = default!;
}
