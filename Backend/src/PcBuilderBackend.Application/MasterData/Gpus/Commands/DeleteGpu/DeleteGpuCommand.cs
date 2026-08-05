using MediatR;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.DeleteGpu;

public record DeleteGpuCommand(Guid Id): IRequest<bool>;