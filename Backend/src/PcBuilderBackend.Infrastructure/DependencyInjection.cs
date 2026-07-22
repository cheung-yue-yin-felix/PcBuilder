using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Infrastructure.Persistence;

namespace PcBuilderBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<PcBuilderDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql => npgsql.MapDomainEnums()));
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<PcBuilderDbContext>() ?? throw new InvalidOperationException());

        return services;
    }
}