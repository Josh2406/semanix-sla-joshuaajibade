namespace Shared.Common
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddSharedCommonServices(this IServiceCollection services)
        {
            services.AddSingleton<IEventBus, InMemoryEventBus>();

            services.AddHttpClient();

            services.AddSingleton<EngineCommsBridge>();
            services.AddSingleton<IInMemoryEventHandler>(sp => sp.GetRequiredService<EngineCommsBridge>());

            return services;
        }
    }
}
