using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkCreateGpuSeries;

public record BulkCreateGpuSeriesCommand(List<GpuSeriesDto> GpuSeries): IRequest<List<GpuSeriesDto>>;