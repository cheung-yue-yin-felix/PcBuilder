using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Psus.Dto;

public record PsuFilter
{
    public string? Name { get; init; } = string.Empty;
    public Guid? ManufacturerId { get; init; }
    public RangeFilter? Wattage { get; init; }
    public PsuModularity? Modularity { get; init; }
    public PsuFormFactor? FormFactor { get; init; }
    public RangeFilter? LengthMm { get; init; }
    public RangeFilter? WidthMm { get; init; }
    public RangeFilter? HeightMm { get; init; }
    public Guid? ChassisId { get; init; }
    public Guid? MotherboardId { get; init; }
    public Guid? GraphicsCardId { get; init; }
    public Guid? CpuId { get; init; }
}
