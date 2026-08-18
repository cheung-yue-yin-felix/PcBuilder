using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;

public record GraphicsCardFilter
{
    public string? Name { get; init; } = string.Empty;
    public Guid? ManufacturerId { get; init; }
    public Guid? GpuId { get; init; }
    public int? VideoMemoryGb { get; init; }
    public int? PcieSlotsUsed { get; init; }
    public PcieGeneration? PcieGeneration { get; init; }
    public RangeFilter? LengthMm { get; init; }
    public RangeFilter? WidthMm { get; init; }
    public RangeFilter? HeightMm { get; init; }
    public RangeFilter? PowerConsumptionWatts { get; init; }
    public Guid? ChassisId { get; init; }
    public Guid? MotherboardId { get; init; }
}