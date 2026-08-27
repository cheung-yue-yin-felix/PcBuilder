using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkCreateCpus;

public record BulkCreateCpusCommand(List<CpuDto> Cpus) : IRequest<List<CpuDto>>;