using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using MediatR;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.CreateSocket;

public record CreateSocketCommand(Guid ManufacturerId, string Name): IRequest<SocketDto>;