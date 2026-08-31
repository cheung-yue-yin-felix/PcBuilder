using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Queries;

public record GetGpuSeriesQuery : IRequest<List<GpuSeriesDto>>;