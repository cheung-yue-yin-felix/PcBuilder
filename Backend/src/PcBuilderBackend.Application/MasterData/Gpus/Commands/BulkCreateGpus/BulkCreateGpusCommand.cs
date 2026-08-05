using MediatR;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.CreateGpu;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkCreateGpus;

public record BulkCreateGpusCommand(List<GpuDto> Gpus): IRequest<List<GpuDto>>;