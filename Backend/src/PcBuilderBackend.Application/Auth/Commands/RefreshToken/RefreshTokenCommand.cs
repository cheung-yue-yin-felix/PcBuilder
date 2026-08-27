using MediatR;
using PcBuilderBackend.Application.Auth.Dto;

namespace PcBuilderBackend.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthTokensDto?>;
