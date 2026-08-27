using MediatR;
using PcBuilderBackend.Application.Auth.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Auth.Queries;

public class GetCurrentUserHandler(ICurrentUser currentUser, IIdentityService identityService)
    : IRequestHandler<GetCurrentUserQuery, CurrentUserDto?>
{
    public Task<CurrentUserDto?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
            return Task.FromResult<CurrentUserDto?>(null);

        return identityService.GetUserAsync(userId, cancellationToken);
    }
}
