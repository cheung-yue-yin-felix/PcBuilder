using MediatR;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkUpdateSockets;

public record BulkUpdateSocketsCommand(List<SocketDto> Sockets): IRequest<List<SocketDto>>;