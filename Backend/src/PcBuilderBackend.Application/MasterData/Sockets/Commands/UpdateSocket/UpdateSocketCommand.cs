using MediatR;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Application.MasterData.Sockets;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.UpdateSocket;

public record UpdateSocketCommand(Guid Id, Guid ManufacturerId, string Name): IRequest<SocketDto?>, ISocketFields;