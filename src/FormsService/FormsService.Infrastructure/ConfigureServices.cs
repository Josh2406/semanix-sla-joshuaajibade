using FormsService.Infrastructure.Metrics;

namespace FormsService.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FormsDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IDbConnection>(_ => new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IQueryRepository, QueryRepository>();
            services.AddSingleton<IFormsMetrics, FormsMetrics>();

            return services;
        }
    }
}
