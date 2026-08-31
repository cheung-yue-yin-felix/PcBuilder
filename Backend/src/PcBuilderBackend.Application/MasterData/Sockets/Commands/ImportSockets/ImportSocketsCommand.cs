using MediatR;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.ImportSockets;

public record ImportSocketsCommand(Stream Stream) : IRequest<List<SocketDto>>;
