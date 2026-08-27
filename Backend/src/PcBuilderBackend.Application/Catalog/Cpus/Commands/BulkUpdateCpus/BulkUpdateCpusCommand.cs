using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpus;

public record BulkUpdateCpusCommand(List<UpdateCpuCommand> Cpus) : IRequest<List<CpuDto>?>;