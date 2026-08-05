using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpus;

public record BulkUpdateCpusCommand(List<CpuDto> Cpus) : IRequest<List<CpuDto>?>;