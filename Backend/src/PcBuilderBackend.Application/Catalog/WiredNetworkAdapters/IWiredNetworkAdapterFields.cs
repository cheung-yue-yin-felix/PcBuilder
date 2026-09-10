using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters;

public interface IWiredNetworkAdapterFields
{
    string Name { get; }
    Guid ManufacturerId { get; }
    WiredHostInterface HostInterface { get; }
    int MaxSpeedMbps { get; }
    UsbVersion? UsbVersion { get; }
    UsbType? UsbType { get; }
    PcieSlotType? PcieSlotType { get; }
}
