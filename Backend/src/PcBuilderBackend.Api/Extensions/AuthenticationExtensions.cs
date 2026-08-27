using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PcBuilderBackend.Api.Auth;
using PcBuilderBackend.Application.Common.Authorization;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Options;

namespace PcBuilderBackend.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException("Jwt configuration section is missing.");
        jwt.Validate();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
                    ClockSkew = TimeSpan.FromSeconds(30),
                    RoleClaimType = System.Security.Claims.ClaimTypes.Role,
                    NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicies.MasterDataWrite, policy => policy.RequireRole(AuthRoles.Admin));
            options.AddPolicy(AuthPolicies.CatalogWrite, policy => policy.RequireRole(AuthRoles.Admin));
            options.AddPolicy(AuthPolicies.BuildWrite, policy => policy.RequireRole(AuthRoles.Member));
        });

        return services;
    }
}
