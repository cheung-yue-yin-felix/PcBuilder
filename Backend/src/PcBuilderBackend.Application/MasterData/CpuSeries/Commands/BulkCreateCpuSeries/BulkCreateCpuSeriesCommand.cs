using MediatR;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkCreateCpuSeries;

public record BulkCreateCpuSeriesCommand(List<CpuSeriesDto> CpuSeries): IRequest<List<CpuSeriesDto>>;