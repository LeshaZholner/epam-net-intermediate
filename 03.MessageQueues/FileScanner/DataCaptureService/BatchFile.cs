using System.Collections;

namespace DataCaptureService;

public class BatchFileEnumerator : IEnumerator<Chunk>
{
    private FileStream? _fileStream;
    private FileInfo _fileInfo;
    private int _chunkSize;


    private Chunk? _current;
    private int _chunkPosition;

    public BatchFileEnumerator(FileInfo fileInfo, int chunkSize)
    {
        _fileInfo = fileInfo;
        _chunkSize = chunkSize;
    }

    public Chunk Current => _current ?? throw new ArgumentException();

    object IEnumerator.Current => throw new NotImplementedException();

    public void Dispose()
    {
        _fileStream?.Dispose();
    }

    public bool MoveNext()
    {
        if (_fileStream == null)
        {
            _fileStream = _fileInfo.OpenRead();
        }

        var buffer = new byte[_chunkSize];
        var numberBytes = _fileStream.Read(buffer, 0, buffer.Length);
        _current = new Chunk
        {
            Position = _chunkPosition++,
            Data = buffer.Take(numberBytes).ToArray()
        };

        return numberBytes > 0;
    }

    public void Reset()
    {
        _fileStream?.Dispose();
        _fileStream = null;
        _current = null;
        _chunkPosition = 0;
    }
}

public class BatchFile : IEnumerable<Chunk>
{
    private readonly FileInfo _fileInfo;
    private readonly int _chunkSize;
    private readonly int _count;

    public BatchFile(string path, int chunkSize)
    {
        _fileInfo = new FileInfo(path);
        _chunkSize = chunkSize;
        _count = GetChunkCount(_chunkSize);
    }

    public BatchFile(FileInfo fileInfo, int chunkSize)
    {
        _fileInfo = fileInfo;
        _chunkSize = chunkSize;
        _count = GetChunkCount(_chunkSize);
    }

    public int Count => _count;

    public IEnumerator<Chunk> GetEnumerator() => new BatchFileEnumerator(_fileInfo, _chunkSize);

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    private int GetChunkCount(int chunkSize)
    {
        var count = (int)(_fileInfo.Length / chunkSize);

        return count % chunkSize > 0 ? count + 1 : count;
    }
}

public class Chunk
{
    public int Position { get; set; }
    public byte[] Data { get; set; } = default!;
}
