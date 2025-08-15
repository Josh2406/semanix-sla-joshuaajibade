using FluentValidation;
using FormsService.Application.Commands;
using FormsService.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace FormsService.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateFormCommand>, CreateFormCommandValidator>();
            services.AddScoped<IValidator<UpdateFormCommand>, UpdateFormCommandValidator>();
           

            return services;
        }
    }
}
