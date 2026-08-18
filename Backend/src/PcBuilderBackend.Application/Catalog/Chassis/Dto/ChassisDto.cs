using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public record ChassisDto : ChassisListItemDto
{
    public List<ChassisFanMountDto> FanMounts { get; init; } = [];
    public List<ChassisDriveBayDto> DriveBays { get; init; } = [];
    public List<ChassisPcieSlotDto> PcieSlots { get; init; } = [];
    public List<ChassisRadiatorDto> Radiators { get; init; } = [];
    public List<PsuFormFactor> PsuFormFactors { get; init; } = [];
    public List<MbFormFactor> MbFormFactors { get; init; } = [];
}