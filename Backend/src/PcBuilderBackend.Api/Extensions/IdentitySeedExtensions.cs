using Microsoft.AspNetCore.Identity;
using PcBuilderBackend.Infrastructure.Identity;
using PcBuilderBackend.Infrastructure.Persistence.Identity;

namespace PcBuilderBackend.Api.Extensions;

public static class IdentitySeedExtensions
{
    public static async Task SeedIdentityAsync(this WebApplication app, CancellationToken cancellationToken = default)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("IdentitySeeder");

        await IdentitySeeder.SeedAsync(db, roleManager, userManager, configuration, logger, cancellationToken);
    }
}
