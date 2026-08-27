using MediatR;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkUpdateCpuSeries;

public record BulkUpdateCpuSeriesCommand(List<CpuSeriesDto> CpuSeries): IRequest<List<CpuSeriesDto>>;