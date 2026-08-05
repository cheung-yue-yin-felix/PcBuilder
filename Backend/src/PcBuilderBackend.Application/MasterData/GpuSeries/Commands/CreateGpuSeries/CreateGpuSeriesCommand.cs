using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.CreateGpuSeries;

public record CreateGpuSeriesCommand(Guid ManufacturerId, string Name): IRequest<GpuSeriesDto>;