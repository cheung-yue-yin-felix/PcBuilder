using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.UpdateGpuSeries;

public record UpdateGpuSeriesCommand(Guid Id, Guid ManufacturerId, string Name): IRequest<GpuSeriesDto?>;