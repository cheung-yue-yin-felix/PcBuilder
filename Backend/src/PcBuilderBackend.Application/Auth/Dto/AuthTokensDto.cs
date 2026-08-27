namespace PcBuilderBackend.Application.Auth.Dto;

public record AuthTokensDto(
    string AccessToken,
    string RefreshToken,
    int ExpiresInSeconds,
    CurrentUserDto User);
