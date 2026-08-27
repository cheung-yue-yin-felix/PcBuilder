namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public class ChassisImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public decimal LengthMm { get; init; }
    public decimal WidthMm { get; init; }
    public decimal HeightMm { get; init; }
    public decimal MotherboardMaxWidthMm { get; init; }
    public decimal MotherboardMaxHeightMm { get; init; }
    public decimal MaxCpuCoolerHeightMm { get; init; }
    public decimal MaxGraphicsCardLengthMm { get; init; }
    public decimal MaxPsuLengthMm { get; init; }
    public List<ChassisDriveBayImportRow> DriveBays { get; init; } = [];
    public List<ChassisFanMountImportRow> FanMounts { get; init; } = [];
    public List<ChassisPcieSlotImportRow> PcieSlots { get; init; } = [];
    public List<ChassisRadiatorImportRow> Radiators { get; init; } = [];
    public List<ChassisMbFormFactorImportRow> MbFormFactors { get; init; } = [];
    public List<ChassisPsuFormFactorImportRow> PsuFormFactors { get; init; } = [];
}
