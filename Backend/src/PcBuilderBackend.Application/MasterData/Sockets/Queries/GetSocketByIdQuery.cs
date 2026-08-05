using MediatR;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Queries;

public record GetSocketByIdQuery(Guid Id) : IRequest<SocketDto?>;