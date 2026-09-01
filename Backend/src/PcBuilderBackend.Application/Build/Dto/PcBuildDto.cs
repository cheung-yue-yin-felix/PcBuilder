namespace PcBuilderBackend.Application.Build.Dto;

public record PcBuildDto : PcBuildListItemDto
{
    public Guid ChassisId { get; init; }
    public Guid MotherboardId { get; init; }
    public Guid CpuId { get; init; }
    public Guid? CpuCoolerId { get; init; }
    public Guid RamKitId { get; init; }
    public Guid? GraphicsCardId { get; init; }
    public Guid PsuId { get; init; }
    public List<PcBuildPartDto> ChassisFans { get; init; } = [];
    public List<PcBuildPartDto> StorageDevices { get; init; } = [];
    public List<PcBuildPartDto> WiredNetworkAdapters { get; init; } = [];
    public List<PcBuildPartDto> WirelessNetworkAdapters { get; init; } = [];
}