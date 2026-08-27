using MediatR;
using PcBuilderBackend.Application.Auth.Dto;

namespace PcBuilderBackend.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResultDto>;
