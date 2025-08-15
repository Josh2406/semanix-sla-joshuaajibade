using FluentValidation;
using FormsService.Application.Commands;
using FormsService.Application.Validators;
using Microsoft.Extensions.DependencyInjection;
using FormsService.Application.Mapping;

namespace FormsService.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateFormCommand>, CreateFormCommandValidator>();
            services.AddScoped<IValidator<UpdateFormCommand>, UpdateFormCommandValidator>();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return services;
        }
    }
}
