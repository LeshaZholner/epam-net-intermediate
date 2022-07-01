namespace ProcessingService
{
    public interface IMessagesBroker
    {
        Task Run<TMessage>(CancellationToken stoppingToken);
    }
}
