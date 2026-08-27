using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;

public class WiredNetworkAdapterImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public WiredHostInterface HostInterface { get; init; }
    public int MaxSpeedMbps { get; init; }
    public UsbVersion? UsbVersion { get; init; }
    public UsbType? UsbType { get; init; }
    public PcieSlotType? PcieSlotType { get; init; }
}
