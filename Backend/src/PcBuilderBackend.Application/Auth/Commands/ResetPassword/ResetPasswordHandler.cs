using MediatR;
using PcBuilderBackend.Application.Auth.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Auth.Commands.ResetPassword;

public class ResetPasswordHandler(IIdentityService identityService, ITokenService tokenService)
    : IRequestHandler<ResetPasswordCommand, IdentityOperationResultDto>
{
    public async Task<IdentityOperationResultDto> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var result = await identityService.ResetPasswordAsync(
            request.Email,
            request.Token,
            request.NewPassword,
            cancellationToken);

        if (result is { Succeeded: true, UserId: { } userId })
            await tokenService.RevokeAllForUserAsync(userId, cancellationToken);

        return result;
    }
}
