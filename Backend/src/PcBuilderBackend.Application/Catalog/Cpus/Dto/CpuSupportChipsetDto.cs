namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public record CpuSupportChipsetDto(
    Guid ChipsetId,
    string ChipsetName,
    bool RequiresBiosUpdate
);