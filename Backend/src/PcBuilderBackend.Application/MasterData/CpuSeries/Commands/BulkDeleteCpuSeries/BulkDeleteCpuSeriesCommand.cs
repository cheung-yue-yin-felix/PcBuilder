using MediatR;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkDeleteCpuSeries;

public record BulkDeleteCpuSeriesCommand(List<Guid> CpuSeriesIds): IRequest<bool>;