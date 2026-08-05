using MediatR;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.ImportGpu;

public record ImportGpusCommand(Stream Stream) : IRequest<List<GpuDto>>;
