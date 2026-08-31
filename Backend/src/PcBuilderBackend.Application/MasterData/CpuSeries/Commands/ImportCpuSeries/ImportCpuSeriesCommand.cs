using MediatR;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.ImportCpuSeries;

public record ImportCpuSeriesCommand(Stream Stream) : IRequest<List<CpuSeriesDto>>;
