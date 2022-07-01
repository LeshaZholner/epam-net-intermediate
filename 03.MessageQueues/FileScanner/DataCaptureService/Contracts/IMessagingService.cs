namespace DataCaptureService;

public interface IMessagingService
{
    Task SendMessageAsync(object message);
}
