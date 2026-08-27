namespace PcBuilderBackend.Application.Auth.Dto;

public record RegisterResultDto(
    bool Succeeded,
    CurrentUserDto? User,
    IReadOnlyDictionary<string, string[]> Errors);
