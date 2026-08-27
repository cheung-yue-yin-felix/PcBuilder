using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.ImportCpu;

public record ImportCpusCommand(Stream Stream) : IRequest<List<CpuDto>>;