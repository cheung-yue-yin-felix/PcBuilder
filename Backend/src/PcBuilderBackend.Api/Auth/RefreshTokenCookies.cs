using PcBuilderBackend.Application.Common.Options;

namespace PcBuilderBackend.Api.Auth;

internal static class RefreshTokenCookies
{
    public const string Name = "refreshToken";

    public static void Set(HttpResponse response, string token, JwtOptions jwt)
    {
        response.Cookies.Append(Name, token, CreateOptions(jwt));
    }

    public static void Delete(HttpResponse response, JwtOptions jwt)
    {
        response.Cookies.Delete(Name, CreateOptions(jwt));
    }

    public static string? Read(HttpRequest request) => request.Cookies[Name];

    private static CookieOptions CreateOptions(JwtOptions jwt) => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Lax,
        Path = "/api/auth",
        Expires = DateTimeOffset.UtcNow.AddDays(jwt.RefreshTokenDays)
    };
}
