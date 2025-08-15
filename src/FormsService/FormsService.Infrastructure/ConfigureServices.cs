using FormsService.Application.Interfaces;
using FormsService.Infrastructure.Persistence;
using FormsService.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common.Contracts;
using Shared.Common.Messaging;
using System.Data;

namespace FormsService.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FormsDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDbConnection>(_ => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IQueryRepository, QueryRepository>();
            services.AddSingleton<IEventBus, InMemoryEventBus>();

            services.AddHttpClient();

            services.AddSingleton<EngineCommsBridge>();
            services.AddSingleton<IInMemoryEventHandler>(sp => sp.GetRequiredService<EngineCommsBridge>());

            return services;
        }
    }
}
