using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PcBuilderBackend.Application.Common.Behaviors;

namespace PcBuilderBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register AutoMapper profiles from this assembly
        services.AddAutoMapper(cfg => {}, typeof(DependencyInjection).Assembly);

        // Register MediatR handlers and pipeline behaviors from this assembly
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        return services;
    }
}
