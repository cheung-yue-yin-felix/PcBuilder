using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

public record CpuCoolerListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public string ManufacturerName { get; init; } = string.Empty;
    public int MaxTdp { get; init; }
    public CpuCoolerType Type { get; init; }
    public decimal? CoolerHeightMm { get; init; }
    public decimal? MaxRamHeightMm { get; init; }
    public RadiatorLength? RadiatorLength { get; init; }
}
