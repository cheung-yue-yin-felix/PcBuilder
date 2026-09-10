using PcBuilderBackend.Application.Build.Dto;

namespace PcBuilderBackend.Application.Build;

public interface IPcBuildFields
{
    string Name { get; }
    string? Description { get; }
    Guid ChassisId { get; }
    Guid MotherboardId { get; }
    Guid CpuId { get; }
    Guid RamKitId { get; }
    Guid PsuId { get; }
    List<PcBuildPartDto> ChassisFans { get; }
    List<PcBuildPartDto> StorageDevices { get; }
    List<PcBuildPartDto> WiredNetworkAdapters { get; }
    List<PcBuildPartDto> WirelessNetworkAdapters { get; }
}
