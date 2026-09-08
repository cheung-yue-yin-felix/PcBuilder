using MediatR;

namespace PcBuilderBackend.Application.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest;
