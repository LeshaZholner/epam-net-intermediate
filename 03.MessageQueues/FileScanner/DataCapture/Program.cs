using RabbitMQ.Client;
using System.Text;

using var watcher = new FileSystemWatcher(@"D:\files");
//using var watcher = new FileSystemWatcher(@"C:\path\to\folder");

watcher.NotifyFilter = NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.FileName
                                | NotifyFilters.DirectoryName;

watcher.Changed += OnChanged;

void OnChanged(object sender, FileSystemEventArgs e)
{
    Console.WriteLine($"{e.FullPath} was update");
}

watcher.Created += OnCreated;

void OnCreated(object sender, FileSystemEventArgs e)
{
    var fi = new FileInfo(e.FullPath);
    if (IsFileLocked(fi))
    {
        Task.Factory.StartNew((path) =>
        {
            while (IsFileLocked(fi))
            {
                Thread.Sleep(10000);
                Console.WriteLine($"{path} check file for open");
            }

            Console.WriteLine($"{path} file can open!");
        }, e.FullPath);
    }
    else
    {
        Console.WriteLine($"{e.FullPath} was created and can open");
    }
}

watcher.Deleted += OnDeleted;

void OnDeleted(object sender, FileSystemEventArgs e)
{
    Console.WriteLine($"{e.FullPath} was deleted");
}

watcher.Renamed += OnRenamed;

void OnRenamed(object sender, RenamedEventArgs e)
{
    Console.WriteLine($"{e.FullPath} was renamed");
}

watcher.Error += OnError;

void OnError(object sender, ErrorEventArgs e)
{
    Console.WriteLine(e.ToString());
}

watcher.Filter = "*.*";
watcher.IncludeSubdirectories = true;
watcher.EnableRaisingEvents = true;

Console.WriteLine("Press enter to exit.");
Console.ReadLine();

bool IsFileLocked(FileInfo fileInfo)
{
    FileStream? stream = null;
    try
    {
        stream = fileInfo.Open(FileMode.Open,
                 FileAccess.ReadWrite, FileShare.None);
    }
    catch (IOException)
    {
        return true;
    }
    finally
    {
        if (stream != null)
            stream.Close();
    }

    return false;
}
//using var fs = new FileStream(@"D:\files\fluid.png", FileMode.Open);
//var fileInfo = new FileInfo(@"D:\files\fluid.png");


//using var fs2 = new FileStream(@"D:\files\fluid2.png", FileMode.CreateNew);


//var buffer = new byte[1024];
//while (fs.Read(buffer, 0, buffer.Length) != 0)
//{
//    Console.WriteLine("read files");
//    fs2.Write(buffer);
//}

//using var fs2 = new FileStream(@"D:\files\fluid2.png", FileMode.CreateNew);

//foreach (var item in GetChunks())
//{
//    Console.WriteLine("Start write chunk");
//    await fs2.WriteAsync(item);
//    Console.WriteLine("End write chunk");
//}

//Console.WriteLine("Copy end");


//byte[] GetChunk()
//{
//    return new byte[1024];
//}

//IEnumerable<byte[]> GetChunks()
//{
//    using var fs = new FileStream(@"D:\files\fluid.png", FileMode.Open);
//    var buffer = new byte[1024];
//    while (fs.Read(buffer, 0, buffer.Length) != 0)
//    {
//        Console.WriteLine("get chunks");
//        yield return buffer;
//    }
//}

//class Chunk
//{
//    public int Number { get; set; }
//    public byte[] Data { get; set; } = default!;
//}

//class Message
//{
//    public string FileName { get; set; }
//    public int ChunkNumber { get; set; }
//    public int Size { get; set; }
//    public byte[] Data { get; set; }
//}


//using var fs = new FileStream(@"D:\files\fluid.png", FileMode.Open);
//var batch = new Batch();
//var fileInfo = new FileInfo(@"D:\files\fluid.png");

//var factory = new ConnectionFactory
//{ 
//    HostName = "40.78.131.71",
//    Port = 5672,
//    UserName = "user",
//    Password = "Caspertesttest1"
//};

//var connection = factory.CreateConnection();
//using var channel = connection.CreateModel();
//channel.ExchangeDeclarePassive("messages");

//var rk = "file_chunk";
//var body = Encoding.UTF8.GetBytes("First Message");
//var properties = channel.CreateBasicProperties();
//properties.MessageId = Guid.NewGuid().ToString("D");
//channel.BasicPublish("messages", rk, properties, body);

//class Chunk
//{
//    public int Number { get; set; }
//    public byte[] Data { get; set; }
//}

//class Batch
//{
//    public int Count { get; set; }
//    IEnumerable<Chunk> Chunks { get; set; }
//}

//class FileWatcherService
//{
//    private IMessagingService _messagingService;
//    private FileSystemWatcher _fileSystemWatcher;

//    public FileWatcherService(IMessagingService messagingService, string path)
//    {
//        _messagingService = messagingService;
//        _fileSystemWatcher = new FileSystemWatcher(path);
//    }


//}

//interface IMessagingService
//{
//    Task SendMessageAsync();
//}

//interface IMessagesBroker
//{
//    Task Run();
//}
