namespace Shared.Common.Messaging
{
    public class InMemoryEventBus(ILogger<InMemoryEventBus> logger, IEnumerable<IInMemoryEventHandler> handlers) : IEventBus
    {
        private readonly ILogger<InMemoryEventBus> _logger = logger;
        private readonly IEnumerable<IInMemoryEventHandler> _handlers = handlers;

        public async Task PublishAsync<T>(T evt, CancellationToken ct = default)
        {
            _logger.LogInformation("Publishing event {EventType}", typeof(T).Name);
            foreach (var h in _handlers)
                await h.HandleAsync(evt!, ct);
        }
    }
}
