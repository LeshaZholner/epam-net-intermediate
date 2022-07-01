using Microsoft.Extensions.Options;
using ProcessingService.Contacts;
using ProcessingService.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace ProcessingService
{
    public class RabbitMqMessagesBroker : IMessagesBroker
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<RabbitMqMessagesBroker> _logger;
        private readonly RabbitMqSettings _settings;

        public RabbitMqMessagesBroker(IServiceScopeFactory serviceScopeFactory,
            ILogger<RabbitMqMessagesBroker> logger,
            IOptions<RabbitMqSettings> options)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _settings = options.Value;
        }

        public async Task Run<TMessage>(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                DispatchConsumersAsync = true
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var queueName = channel.QueueDeclarePassive(_settings.RouteKey).QueueName;

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += (_, args) => MessageHandler<TMessage>(args, channel);
            channel.BasicConsume(queueName, false, consumer);
            
            var waitApp = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            await using var _ = stoppingToken.Register(() => waitApp.SetResult());
            await waitApp.Task;

            channel.BasicCancel(consumer.ConsumerTags[0]);
        }

        private async Task MessageHandler<TMessage>(BasicDeliverEventArgs arg, IModel channel)
        {
            _logger.LogInformation("Message received '{messageId}'", arg.BasicProperties.MessageId);

            if(!TryDeserializeMessage<TMessage>(arg.Body, arg.BasicProperties.MessageId, out var message) 
                || message == null)
            {
                channel.BasicReject(arg.DeliveryTag, false);
                return;
            }

            var success = await SaveChunk();

            if (success)
            {
                channel.BasicAck(arg.DeliveryTag, false);
            }
            else
            {
                channel.BasicReject(arg.DeliveryTag, false);
            }

            async Task<bool> SaveChunk()
            {
                await using var scope = _serviceScopeFactory.CreateAsyncScope();
                var service = scope.ServiceProvider.GetRequiredService<ISaveService<TMessage>>();

                return await service.SaveMessageAsync(message);
            }
        }

        private bool TryDeserializeMessage<TMessage>(ReadOnlyMemory<byte> body, string messageId, out TMessage? value)
        {
            _logger.LogInformation("Deserialize message '{messageId}'", messageId);

            try
            {
                var encodedMessage = Encoding.UTF8.GetString(body.ToArray());
                value = JsonSerializer.Deserialize<TMessage>(encodedMessage);

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to deserialize message 'messageId'", messageId);
                value = default;

                return false;
            }
        }
    }
}
