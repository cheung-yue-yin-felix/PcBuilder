using MediatR;
using PcBuilderBackend.Application.Auth.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Auth.Commands.Login;

public class LoginHandler(IIdentityService identityService, ITokenService tokenService)
    : IRequestHandler<LoginCommand, LoginResultDto>
{
    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var signIn = await identityService.PasswordSignInAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (signIn.IsLockedOut)
            return new LoginResultDto(false, true, null);

        if (!signIn.Succeeded || signIn.User is null)
            return new LoginResultDto(false, false, null);

        var tokens = await tokenService.IssueTokenPairAsync(signIn.User, cancellationToken);
        return new LoginResultDto(true, false, tokens);
    }
}
