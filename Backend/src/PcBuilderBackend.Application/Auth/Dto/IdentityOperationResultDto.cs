namespace PcBuilderBackend.Application.Auth.Dto;

public record IdentityOperationResultDto(
    bool Succeeded,
    Guid? UserId,
    IReadOnlyDictionary<string, string[]> Errors);
