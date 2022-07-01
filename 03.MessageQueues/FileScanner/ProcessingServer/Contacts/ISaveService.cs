namespace ProcessingService.Contacts;

public interface ISaveService<TMessage>
{
    Task<bool> SaveMessageAsync(TMessage message);
}
