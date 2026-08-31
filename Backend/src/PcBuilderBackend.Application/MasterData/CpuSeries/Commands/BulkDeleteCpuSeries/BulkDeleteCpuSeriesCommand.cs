using MediatR;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkDeleteCpuSeries;

public record BulkDeleteCpuSeriesCommand(List<Guid> Ids) : IRequest<bool>;