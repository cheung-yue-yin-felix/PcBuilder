using MediatR;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkCreateSockets;

public record BulkCreateSocketsCommand(List<SocketDto> Sockets): IRequest<List<SocketDto>>;