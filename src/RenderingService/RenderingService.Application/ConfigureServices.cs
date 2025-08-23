using Microsoft.Extensions.DependencyInjection;
using RenderingService.Application.Implementations;
using RenderingService.Application.Interfaces;
using RenderingService.Application.Mappings;

namespace RenderingService.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRenderingServices, RenderingServices>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return services;
        }
    }
}
