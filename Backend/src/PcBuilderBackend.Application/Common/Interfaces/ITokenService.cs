using PcBuilderBackend.Application.Auth.Dto;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface ITokenService
{
    Task<AuthTokensDto> IssueTokenPairAsync(CurrentUserDto user, CancellationToken cancellationToken);

    Task<AuthTokensDto?> RotateAsync(string refreshToken, CancellationToken cancellationToken);

    Task RevokeAsync(string refreshToken, CancellationToken cancellationToken);

    Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken);
}
