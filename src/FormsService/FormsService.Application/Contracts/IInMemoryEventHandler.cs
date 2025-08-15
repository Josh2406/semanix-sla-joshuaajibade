namespace FormsService.Application.Contracts
{
    public interface IInMemoryEventHandler
    {
        Task HandleAsync(object evt, CancellationToken token);
    }
}
