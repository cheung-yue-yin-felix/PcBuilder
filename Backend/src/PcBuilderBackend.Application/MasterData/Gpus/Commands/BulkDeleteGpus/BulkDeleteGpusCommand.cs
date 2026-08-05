using MediatR;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkDeleteGpus;

public record BulkDeleteGpusCommand(List<Guid> GpuIds) : IRequest<bool>;