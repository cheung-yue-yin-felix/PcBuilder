using MediatR;
using PcBuilderBackend.Application.Auth.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Auth.Commands.RefreshToken;

public class RefreshTokenHandler(ITokenService tokenService)
    : IRequestHandler<RefreshTokenCommand, AuthTokensDto?>
{
    public Task<AuthTokensDto?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return tokenService.RotateAsync(request.RefreshToken, cancellationToken);
    }
}
