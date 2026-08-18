using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpuSupportChipsets;

public record BulkUpdateCpuSupportChipsetsCommand(Guid CpuId, List<CpuSupportChipsetDto> SupportChipsets)
    : IRequest<List<CpuSupportChipsetDto>?>;
