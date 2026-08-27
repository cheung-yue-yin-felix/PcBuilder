using PcBuilderBackend.Application.Common.Options;

namespace PcBuilderBackend.Api.Auth;

internal static class RefreshTokenCookies
{
    public const string Name = "refreshToken";

    public static void Set(HttpResponse response, string token, JwtOptions jwt, bool secure)
    {
        response.Cookies.Append(Name, token, CreateOptions(jwt, secure));
    }

    public static void Delete(HttpResponse response, JwtOptions jwt, bool secure)
    {
        response.Cookies.Delete(Name, CreateOptions(jwt, secure));
    }

    public static string? Read(HttpRequest request) => request.Cookies[Name];

    private static CookieOptions CreateOptions(JwtOptions jwt, bool secure) => new()
    {
        HttpOnly = true,
        Secure = secure,
        SameSite = SameSiteMode.Lax,
        Path = "/api/auth",
        Expires = DateTimeOffset.UtcNow.AddDays(jwt.RefreshTokenDays)
    };
}
