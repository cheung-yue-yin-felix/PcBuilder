using MediatR;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkDeleteGpuSeries;

public record BulkDeleteGpuSeriesCommand(List<Guid> Ids): IRequest<bool>;