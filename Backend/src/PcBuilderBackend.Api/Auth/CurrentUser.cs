using System.Security.Claims;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Api.Auth;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid? UserId =>
        Guid.TryParse(
            httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out var userId)
            ? userId
            : null;

    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public bool IsInRole(string role) =>
        httpContextAccessor.HttpContext?.User.IsInRole(role) == true;
}
