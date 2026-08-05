using MediatR;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.DeleteCpuSeries;

public record DeleteCpuSeriesCommand(Guid CpuSeriesId) : IRequest<bool>;