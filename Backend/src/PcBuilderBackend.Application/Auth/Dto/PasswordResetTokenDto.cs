namespace PcBuilderBackend.Application.Auth.Dto;

public record PasswordResetTokenDto(Guid UserId, string Email, string Token);
