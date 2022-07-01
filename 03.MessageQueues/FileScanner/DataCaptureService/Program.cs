using DataCaptureService;
using DataCaptureService.Contracts;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<RabbitMqSettings>(hostContext.Configuration.GetSection(nameof(RabbitMqSettings)));
        services.Configure<FileSystemWatcherSettings>(hostContext.Configuration.GetSection(nameof(FileSystemWatcherSettings)));
        services.AddTransient<IMessagingService, RabbitMqMessagingService>();
        services.AddTransient<IFileSystemWatcherService, MqFileSystemWatcherService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
