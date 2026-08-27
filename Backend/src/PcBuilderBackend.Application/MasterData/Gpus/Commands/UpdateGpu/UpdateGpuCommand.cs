using MediatR;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.UpdateGpu;

public record UpdateGpuCommand(Guid Id, Guid ManufacturerId, Guid GpuSeriesId, string Name): IRequest<GpuDto?>;