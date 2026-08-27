using MediatR;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Queries;

public record GetCpuSeriesByIdQuery(Guid CpuSeriesId): IRequest<CpuSeriesDto?>;