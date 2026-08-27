using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PcBuilderBackend.Application.Catalog.Chassis;
using PcBuilderBackend.Application.Catalog.CpuCoolers;
using PcBuilderBackend.Application.Catalog.Cpus;
using PcBuilderBackend.Application.Catalog.GraphicsCards;
using PcBuilderBackend.Application.Catalog.Memories;
using PcBuilderBackend.Application.Catalog.Motherboards;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Options;
using PcBuilderBackend.Infrastructure.Caching;
using PcBuilderBackend.Infrastructure.Identity;
using PcBuilderBackend.Infrastructure.Persistence;
using PcBuilderBackend.Infrastructure.Persistence.Identity;
using PcBuilderBackend.Infrastructure.Persistence.Queries;
using PcBuilderBackend.Infrastructure.Persistence.Repositories;
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
        var identityConnectionString = configuration.GetConnectionString("IdentityConnection")
            ?? throw new InvalidOperationException("Connection string 'IdentityConnection' was not found.");

        services.AddDbContext<PcBuilderDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql => npgsql.MapDomainEnums()));

        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseNpgsql(identityConnectionString, npgsql =>
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "identity")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetService<PcBuilderDbContext>()
            ?? throw new InvalidOperationException());
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IActiveEntityLookup, ActiveEntityLookup>();

        // Read Stores
        services.AddScoped<IChassisReadStore, ChassisReadStore>();
        services.AddScoped<ICpuReadStore, CpuReadStore>();
        services.AddScoped<ICpuCoolerReadStore, CpuCoolerReadStore>();
        services.AddScoped<IMotherboardReadStore, MotherboardReadStore>();
        services.AddScoped<IRamReadStore, RamReadStore>();
        services.AddScoped<IGraphicsCardReadStore, GraphicsCardReadStore>();
        
        // Repositories
        services.AddScoped<IChassisRepository, ChassisRepository>();
        services.AddScoped<ICpuRepository, CpuRepository>();
        services.AddScoped<ICpuCoolerRepository, CpuCoolerRepository>();
        services.AddScoped<IMotherboardRepository, MotherboardRepository>();
        services.AddScoped<IRamRepository, RamRepository>();
        services.AddScoped<IGraphicsCardRepository, GraphicsCardRepository>();

        services.AddScoped<IExcelImportService, ClosedXmlExcelImportService>();

        AddIdentity(services);
        AddJwt(services, configuration);
        AddRedisCache(services, configuration);

        return services;
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 10;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();
    }

    private static void AddJwt(IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException("Jwt configuration section is missing.");
        jwt.Validate();
        services.AddSingleton(jwt);
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
