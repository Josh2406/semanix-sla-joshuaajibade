namespace Shared.Common.Contracts
{
    public interface IInMemoryEventHandler
    {
        Task HandleAsync(object evt, CancellationToken token);
    }
}
