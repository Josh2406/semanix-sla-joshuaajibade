using Microsoft.Extensions.DependencyInjection;
using RenderingService.Application.Implementations;
using RenderingService.Application.Interfaces;

namespace RenderingService.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRenderingServices, RenderingServices>();
            return services;
        }
    }
}
