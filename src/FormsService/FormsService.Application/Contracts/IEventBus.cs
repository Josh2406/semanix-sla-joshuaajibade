namespace FormsService.Application.Contracts
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T evt, CancellationToken token = default);
    }
}
