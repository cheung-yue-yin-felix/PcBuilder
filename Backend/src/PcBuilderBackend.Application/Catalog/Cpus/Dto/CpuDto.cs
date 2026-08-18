namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public record CpuDto : CpuListItemDto
{
    public List<CpuRamCompatDto> RamCompats { get; init; } = [];
    public List<CpuSupportChipsetDto> SupportChipsets { get; init; } = [];
}
