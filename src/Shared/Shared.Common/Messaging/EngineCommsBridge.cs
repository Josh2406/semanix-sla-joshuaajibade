namespace Shared.Common.Messaging 
{
    public class EngineCommsBridge(IHttpClientFactory httpFactory, IConfiguration configuration, ILogger<EngineCommsBridge> logger) 
        : IInMemoryEventHandler
    {
        private readonly IHttpClientFactory _httpFactory = httpFactory;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<EngineCommsBridge> _logger = logger;

        public async Task HandleAsync(object evt, CancellationToken token)
        {
            var client = _httpFactory.CreateClient();
            var renderingBase = _configuration.GetValue<string>("RenderingServiceBaseUrl") ?? "https://localhost:7266";
            var url = $"{renderingBase}/events/forms";
            var resp = await client.PostAsJsonAsync(url, evt, token);
            if (!resp.IsSuccessStatusCode)
                _logger.LogWarning("RenderingService rejected event: {Status}", resp.StatusCode);
        }
    }
}
