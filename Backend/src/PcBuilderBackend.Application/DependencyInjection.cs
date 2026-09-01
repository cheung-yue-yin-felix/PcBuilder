using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PcBuilderBackend.Application.Build;
using PcBuilderBackend.Application.Common.Behaviors;

namespace PcBuilderBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Register AutoMapper profiles from this assembly
        //services.AddAutoMapper(cfg => {}, typeof(DependencyInjection).Assembly);

        services.AddAutoMapper(cfg =>
        {
            cfg.LicenseKey = configuration["AutoMapper:LicenseKey"];
            cfg.AddMaps(typeof(DependencyInjection).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<ICompatibilityChecker, CompatibilityChecker>();

        // Register MediatR handlers and pipeline behaviors from this assembly
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.LicenseKey = configuration["MediatR:LicenseKey"];
        });

        return services;
    }
}
