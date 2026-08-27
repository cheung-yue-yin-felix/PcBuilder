using MediatR;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.DeleteSocket;

public record DeleteSocketCommand(Guid Id): IRequest<bool>;