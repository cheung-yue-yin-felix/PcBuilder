using MediatR;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkUpdateGpus;

public record BulkUpdateGpusCommand(List<GpuDto> Gpus): IRequest<List<GpuDto>>;