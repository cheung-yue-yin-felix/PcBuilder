using MediatR;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;
using PcBuilderBackend.Application.MasterData.GpuSeries;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.CreateGpuSeries;

public record CreateGpuSeriesCommand(Guid ManufacturerId, string Name): IRequest<GpuSeriesDto>, IGpuSeriesFields;