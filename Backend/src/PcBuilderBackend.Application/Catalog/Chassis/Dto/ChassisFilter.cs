using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public record ChassisFilter
{
    public string? Name { get; init; }
    public Guid? ManufacturerId { get; init; }
    public RangeFilter? LengthMm { get; init; } = new();
    public RangeFilter? WidthMm { get; init; } = new();
    public RangeFilter? HeightMm { get; init; } = new();
    public RangeFilter? MotherboardMaxWidthMm { get; init; } = new();
    public RangeFilter? MotherboardMaxHeightMm { get; init; } = new();
    public RangeFilter? MaxCpuCoolerHeightMm { get; init; } = new();
    public RangeFilter? MaxGraphicsCardLengthMm { get; init; } = new();
    public RangeFilter? MaxPsuLengthMm { get; init; } = new();
    public List<MbFormFactor> SupportedMbFormFactors { get; init; } = new();
}