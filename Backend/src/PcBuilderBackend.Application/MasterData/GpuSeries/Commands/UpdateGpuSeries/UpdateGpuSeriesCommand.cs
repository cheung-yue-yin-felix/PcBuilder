using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;
using PcBuilderBackend.Application.MasterData.GpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.UpdateGpuSeries;

public record UpdateGpuSeriesCommand(Guid Id, Guid ManufacturerId, string Name): IRequest<GpuSeriesDto?>, IGpuSeriesFields;