using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Auth.Dto;
using PcBuilderBackend.Application.Common.Authorization;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Infrastructure.Persistence.Identity;

namespace PcBuilderBackend.Infrastructure.Identity;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    ILogger<IdentityService> logger) : IIdentityService
{
    public async Task<RegisterResultDto> RegisterMemberAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim()
        };

        var createResult = await userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
            return new RegisterResultDto(false, null, ToErrors(createResult));

        var roleResult = await userManager.AddToRoleAsync(user, AuthRoles.Member);
        if (!roleResult.Succeeded)
            return new RegisterResultDto(false, null, ToErrors(roleResult));

        IdentityLog.Registered(logger, user.Id, user.Email ?? string.Empty);
        return new RegisterResultDto(true, await MapAsync(user), new Dictionary<string, string[]>());
    }

    public async Task<PasswordSignInResultDto> PasswordSignInAsync(
        string email,
        string password,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            IdentityLog.SignInFailed(logger, email);
            return new PasswordSignInResultDto(false, false, null);
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            IdentityLog.SignInLockedOut(logger, email);
            return new PasswordSignInResultDto(false, true, null);
        }

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            await userManager.AccessFailedAsync(user);
            if (await userManager.IsLockedOutAsync(user))
            {
                IdentityLog.SignInLockedOut(logger, email);
                return new PasswordSignInResultDto(false, true, null);
            }

            IdentityLog.SignInFailed(logger, email);
            return new PasswordSignInResultDto(false, false, null);
        }

        await userManager.ResetAccessFailedCountAsync(user);
        IdentityLog.SignedIn(logger, user.Id);
        return new PasswordSignInResultDto(true, false, await MapAsync(user));
    }

    public async Task<CurrentUserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is null ? null : await MapAsync(user);
    }

    private async Task<CurrentUserDto> MapAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        return new CurrentUserDto(user.Id, user.Email!, user.FirstName, user.LastName, roles.ToList());
    }

    private static Dictionary<string, string[]> ToErrors(IdentityResult result)
    {
        return result.Errors
            .GroupBy(error => error.Code switch
            {
                "DuplicateEmail" or "InvalidEmail" or "DuplicateUserName" or "InvalidUserName" => "email",
                _ when error.Code.Contains("Password", StringComparison.OrdinalIgnoreCase) => "password",
                _ => "identity"
            })
            .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray());
    }
}
