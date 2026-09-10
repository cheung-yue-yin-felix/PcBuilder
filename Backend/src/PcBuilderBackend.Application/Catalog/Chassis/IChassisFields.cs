using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis;

public interface IChassisFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    decimal LengthMm { get; }
    decimal WidthMm { get; }
    decimal HeightMm { get; }
    decimal MotherboardMaxWidthMm { get; }
    decimal MotherboardMaxHeightMm { get; }
    decimal MaxCpuCoolerHeightMm { get; }
    decimal MaxGraphicsCardLengthMm { get; }
    decimal MaxPsuLengthMm { get; }
}

public interface IChassisCollections
{
    List<ChassisFanMountDto> FanMounts { get; }
    List<ChassisDriveBayDto> DriveBays { get; }
    List<ChassisPcieSlotDto> PcieSlots { get; }
    List<ChassisRadiatorDto> Radiators { get; }
    List<PsuFormFactor> PsuFormFactors { get; }
    List<MbFormFactor> MbFormFactors { get; }
}
