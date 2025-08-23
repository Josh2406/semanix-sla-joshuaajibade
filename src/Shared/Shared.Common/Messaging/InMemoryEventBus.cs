namespace Shared.Common.Messaging
{
    public class InMemoryEventBus(ILogger<InMemoryEventBus> logger, IInMemoryEventHandler handler) : IEventBus
    {
        private readonly ILogger<InMemoryEventBus> _logger = logger;
        private readonly IInMemoryEventHandler _handler = handler;

        public async Task PublishAsync<T>(T evt, CancellationToken ct = default)
        {
            if(_handler != null)
            {
                _logger.LogInformation("Publishing event {EventType}", typeof(T).Name);
                await _handler.HandleAsync(evt!, ct);
            }
        }
    }
}
