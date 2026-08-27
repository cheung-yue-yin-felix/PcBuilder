using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Queries;

public record GetGpuSeriesByIdQuery(Guid Id): IRequest<GpuSeriesDto?>;
