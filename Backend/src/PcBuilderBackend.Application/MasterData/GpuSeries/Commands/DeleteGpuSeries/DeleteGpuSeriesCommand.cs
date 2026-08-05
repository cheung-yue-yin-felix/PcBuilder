using MediatR;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.DeleteGpuSeries;

public record DeleteGpuSeriesCommand(Guid Id): IRequest<bool>;