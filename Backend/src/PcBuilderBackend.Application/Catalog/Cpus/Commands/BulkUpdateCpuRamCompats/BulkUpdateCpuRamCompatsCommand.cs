using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpuRamCompats;

public record BulkUpdateCpuRamCompatsCommand(Guid CpuId, List<CpuRamCompatDto> RamCompats)
    : IRequest<List<CpuRamCompatDto>?>;
