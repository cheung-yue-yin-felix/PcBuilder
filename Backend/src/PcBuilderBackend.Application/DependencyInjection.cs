using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PcBuilderBackend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register AutoMapper profiles from this assembly
        services.AddAutoMapper(cfg => {}, typeof(DependencyInjection).Assembly);

        // Register MediatR handlers from this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
