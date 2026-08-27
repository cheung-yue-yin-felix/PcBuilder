using MediatR;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.BulkDeleteSockets;

public record BulkDeleteSocketsCommand(List<Guid> SocketIds): IRequest<bool>;