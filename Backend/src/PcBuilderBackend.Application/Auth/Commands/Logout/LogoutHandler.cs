using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Auth.Commands.Logout;

public class LogoutHandler(ITokenService tokenService) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return;

        await tokenService.RevokeAsync(request.RefreshToken, cancellationToken);
    }
}
