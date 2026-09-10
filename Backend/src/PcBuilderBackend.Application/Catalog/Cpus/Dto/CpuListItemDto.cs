using PcBuilderBackend.Application.Catalog.Cpus;

namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public record CpuListItemDto : ICpuFields
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public string ManufacturerName { get; init; } = string.Empty;
    public Guid SeriesId { get; init; }
    public string SeriesName { get; init; } = string.Empty;
    public Guid SocketId { get; init; }
    public string SocketName { get; init; } = string.Empty;
    public int MaxMemoryGb { get; init; }
    public bool IntegratedGraphics { get; init; }
    public bool IncludedStockCooler { get; init; }
    public int ThermalDesignPower { get; init; }
    public int PowerConsumptionWatts { get; init; }
}
