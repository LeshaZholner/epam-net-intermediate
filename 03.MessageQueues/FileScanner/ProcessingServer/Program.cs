using ProcessingService;
using ProcessingService.Contacts;
using ProcessingService.Messages;
using ProcessingService.Services;
using ProcessingService.Settings;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<RabbitMqSettings>(hostContext.Configuration.GetSection(nameof(RabbitMqSettings)));
        services.Configure<FileChunkSaveSettings>(hostContext.Configuration.GetSection(nameof(FileChunkSaveSettings)));
        services.AddTransient<ISaveService<FileChunk>, FileChunkSaveService>();

        services.AddSingleton<IMessagesBroker, RabbitMqMessagesBroker>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
