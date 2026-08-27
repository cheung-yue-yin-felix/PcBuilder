using MediatR;
using PcBuilderBackend.Application.Auth.Dto;

namespace PcBuilderBackend.Application.Auth.Queries;

public record GetCurrentUserQuery : IRequest<CurrentUserDto?>;
