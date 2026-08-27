using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;

public record WiredNetworkAdapterFilter
{
    public string? Name { get; init; } = string.Empty;
    public Guid? ManufacturerId { get; init; }
    public WiredHostInterface? HostInterface { get; init; }
    public RangeFilter? MaxSpeedMbps { get; init; }
    public UsbVersion? UsbVersion { get; init; }
    public UsbType? UsbType { get; init; }
    public PcieSlotType? PcieSlotType { get; init; }
    public Guid? MotherboardId { get; init; }
}
