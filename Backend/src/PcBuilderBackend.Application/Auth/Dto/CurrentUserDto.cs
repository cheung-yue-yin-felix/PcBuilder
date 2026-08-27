namespace PcBuilderBackend.Application.Auth.Dto;

public record CurrentUserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    IReadOnlyList<string> Roles);
