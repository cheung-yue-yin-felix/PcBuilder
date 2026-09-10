using MediatR;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.MasterData.Gpus;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.CreateGpu;

public record CreateGpuCommand(Guid ManufacturerId, Guid GpuSeriesId, string Name): IRequest<GpuDto>, IGpuFields;