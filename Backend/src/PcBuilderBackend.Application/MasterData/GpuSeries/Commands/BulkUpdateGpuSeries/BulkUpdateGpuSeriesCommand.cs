using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkUpdateGpuSeries;

public record BulkUpdateGpuSeriesCommand(List<GpuSeriesDto> GpuSeries): IRequest<List<GpuSeriesDto>>;