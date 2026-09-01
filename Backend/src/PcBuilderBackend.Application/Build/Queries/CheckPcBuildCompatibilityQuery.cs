using MediatR;
using PcBuilderBackend.Application.Build.Dto;

namespace PcBuilderBackend.Application.Build.Queries;

public record CheckPcBuildCompatibilityQuery(
    Guid? ChassisId,
    Guid? MotherboardId,
    Guid? CpuId,
    Guid? CpuCoolerId,
    Guid? RamKitId,
    Guid? GraphicsCardId,
    Guid? PsuId,
    List<PcBuildPartDto>? ChassisFans,
    List<PcBuildPartDto>? StorageDevices,
    List<PcBuildPartDto>? WiredNetworkAdapters,
    List<PcBuildPartDto>? WirelessNetworkAdapters) : IRequest<CompatibilityCheckDto>;
