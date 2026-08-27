using MediatR;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.CreateCpuSeries;

public record CreateCpuSeriesCommand(string Name, Guid ManufacturerId, Guid SocketId): IRequest<CpuSeriesDto>;