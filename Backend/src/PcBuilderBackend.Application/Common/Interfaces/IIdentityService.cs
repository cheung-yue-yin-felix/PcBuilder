using PcBuilderBackend.Application.Auth.Dto;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<RegisterResultDto> RegisterMemberAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        CancellationToken cancellationToken);

    Task<PasswordSignInResultDto> PasswordSignInAsync(
        string email,
        string password,
        CancellationToken cancellationToken);

    Task<CurrentUserDto?> GetUserAsync(Guid userId, CancellationToken cancellationToken);
}

public record PasswordSignInResultDto(
    bool Succeeded,
    bool IsLockedOut,
    CurrentUserDto? User);
