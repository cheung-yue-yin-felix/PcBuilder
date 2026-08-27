using MediatR;

namespace PcBuilderBackend.Application.Auth.Commands.Logout;

public record LogoutCommand(string? RefreshToken) : IRequest;
