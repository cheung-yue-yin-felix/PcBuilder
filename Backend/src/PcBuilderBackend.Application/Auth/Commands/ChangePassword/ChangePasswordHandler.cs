using MediatR;
using PcBuilderBackend.Application.Auth.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Auth.Commands.ChangePassword;

public class ChangePasswordHandler(
    IIdentityService identityService,
    ICurrentUser currentUser,
    ITokenService tokenService) : IRequestHandler<ChangePasswordCommand, IdentityOperationResultDto>
{
    public async Task<IdentityOperationResultDto> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
        {
            return new IdentityOperationResultDto(
                false,
                null,
                new Dictionary<string, string[]>
                {
                    ["identity"] = ["You must be signed in to change your password."]
                });
        }

        var result = await identityService.ChangePasswordAsync(
            userId,
            request.CurrentPassword,
            request.NewPassword,
            cancellationToken);

        if (result is { Succeeded: true, UserId: { } })
            await tokenService.RevokeAllForUserAsync(userId, cancellationToken);

        return result;
    }
}
