using MediatR;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.UpdateCpuSeries;

public record UpdateCpuSeriesCommand(Guid CpuSeriesId, string Name, Guid ManufacturerId, Guid SocketId) : IRequest<CpuSeriesDto?>;