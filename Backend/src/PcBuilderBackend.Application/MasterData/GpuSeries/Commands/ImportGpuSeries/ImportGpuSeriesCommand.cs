using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.ImportGpuSeries;

public record ImportGpuSeriesCommand(Stream Stream) : IRequest<List<GpuSeriesDto>>;
