using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DataCaptureService;

public class RabbitMqMessagingService : IMessagingService
{
    private readonly RabbitMqSettings _settings;
    
    public RabbitMqMessagingService(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
    }

    public Task SendMessageAsync(object message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclarePassive(_settings.Exchange);

        var routingKey = _settings.RouteKey;
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var properties = channel.CreateBasicProperties();
        properties.MessageId = Guid.NewGuid().ToString("D");
        channel.BasicPublish(_settings.Exchange, routingKey, properties, body);

        return Task.CompletedTask;
    }
}
