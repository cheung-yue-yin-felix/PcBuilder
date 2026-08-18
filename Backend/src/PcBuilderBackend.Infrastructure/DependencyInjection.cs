using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Infrastructure.Caching;
using PcBuilderBackend.Infrastructure.Persistence;
using PcBuilderBackend.Infrastructure.Services;
using StackExchange.Redis;

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

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetService<PcBuilderDbContext>()
            ?? throw new InvalidOperationException());
        services.AddScoped<IExcelImportService, ClosedXmlExcelImportService>();

        AddRedisCache(services, configuration);

        return services;
    }

    private static void AddRedisCache(IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection =
            configuration.GetConnectionString("Redis")
            ?? configuration["Redis:Configuration"]
            ?? "localhost:6379";

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConnection));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "PcBuilder:";
        });

        services.AddSingleton<ICacheService, RedisCacheService>();
    }
}
