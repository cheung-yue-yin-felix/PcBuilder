using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PcBuilderBackend.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PcBuilderDbContext>
{
    public PcBuilderDbContext CreateDbContext(string[] args)
    {
        var conn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (string.IsNullOrWhiteSpace(conn))
        {
            var apiPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "PcBuilderBackend.Api"));
            var config = new ConfigurationBuilder()
                .SetBasePath(apiPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            conn = config.GetConnectionString("DefaultConnection");
        }

        if (string.IsNullOrWhiteSpace(conn))
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Set env var ConnectionStrings__DefaultConnection or add it to Api appsettings.");

        var options = new DbContextOptionsBuilder<PcBuilderDbContext>()
            .UseNpgsql(conn, npgsql => npgsql.MapDomainEnums())
            .Options;

        return new PcBuilderDbContext(options);
    }
}