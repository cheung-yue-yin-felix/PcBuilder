namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public record ChassisListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public string ManufacturerName { get; init; } = string.Empty;
    public decimal LengthMm { get; init; }
    public decimal WidthMm { get; init; }
    public decimal HeightMm { get; init; }
    public decimal MotherboardMaxWidthMm { get; init; }
    public decimal MotherboardMaxHeightMm { get; init; }
    public decimal MaxCpuCoolerHeightMm { get; init; }
    public decimal MaxGraphicsCardLengthMm { get; init; }
    public decimal MaxPsuLengthMm { get; init; }
}
    
