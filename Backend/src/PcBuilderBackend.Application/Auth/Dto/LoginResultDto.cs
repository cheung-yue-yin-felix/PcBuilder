namespace PcBuilderBackend.Application.Auth.Dto;

public record LoginResultDto(
    bool Succeeded,
    bool IsLockedOut,
    AuthTokensDto? Tokens);
